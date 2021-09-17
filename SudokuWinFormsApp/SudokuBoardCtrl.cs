using System.Drawing;
using System.Windows.Forms;

namespace PendleCodeMonkey.SudokuWinFormsApp
{
	/// <summary>
	/// A sudoku board control.
	/// </summary>
	public class SudokuBoardCtrl : Panel
	{

		private int[,] _board = new int[9, 9];
		private int[,] _initialBoard = new int[9, 9];
		private int[,] _solutionBoard = new int[9, 9];
		private readonly string _numbers = "123456789";

		public int HEADER_HEIGHT = 40;

		private int _selectedDigit = 1;
		private bool _inShowErrorsMode = false;

		private Point _highlightedSquare = new Point(-1, -1);

		public delegate void MouseEventDelegate(int row, int col, int digit);

		private MouseEventDelegate _mouseClickDel = null;

		public bool InShowErrorsMode
		{
			get
			{
				return _inShowErrorsMode;
			}
			set
			{
				_inShowErrorsMode = value;
			}
		}

		public void SetBoard(int[,] board, bool setInitialBoard = false)
		{
			DoubleBuffered = true;

			_board = new int[9, 9];
			if (setInitialBoard)
			{
				_initialBoard = new int[9, 9];
			}
			for (int row = 0; row < 9; row++)
			{
				for (int col = 0; col < 9; col++)
				{
					_board[row, col] = board[row, col];
					if (setInitialBoard)
					{
						_initialBoard[row, col] = board[row, col];
					}
				}
			}

			Invalidate();
		}

		public void SetBoard(int[] board, bool setInitialBoard = false)
		{
			DoubleBuffered = true;

			_board = new int[9, 9];
			if (setInitialBoard)
			{
				_initialBoard = new int[9, 9];
			}
			for (int row = 0; row < 9; row++)
			{
				for (int col = 0; col < 9; col++)
				{
					_board[row, col] = board[(row * 9) + col];
					if (setInitialBoard)
					{
						_initialBoard[row, col] = board[(row * 9) + col];
					}
				}
			}

			Invalidate();
		}

		public void SetSolutionBoard(int[] solution)
		{
			_solutionBoard = new int[9, 9];
			for (int row = 0; row < 9; row++)
			{
				for (int col = 0; col < 9; col++)
				{
					_solutionBoard[row, col] = solution[(row * 9) + col];
				}
			}
		}

		public void AttachEventHandlers(MouseEventDelegate mouseClickDel)
		{
			DoubleBuffered = true;

			_mouseClickDel = mouseClickDel;

			this.MouseClick += Ctrl_MouseClick;
			this.MouseMove += Ctrl_MouseMove;
			this.MouseLeave += Ctrl_MouseLeave;
		}

		public void DetachEventHandlers()
		{
			this.MouseClick -= Ctrl_MouseClick;
			this.MouseMove -= Ctrl_MouseMove;
			this.MouseLeave -= Ctrl_MouseLeave;
		}

		public void SetSelectedDigit(int digit)
		{
			if (digit >= 1 && digit <= 9)
			{
				_selectedDigit = digit;
				Invalidate();
			}
		}

		public int GetNumberOfMistakes()
		{
			int mistakes = 0;
			for (int row = 0; row < 9; row++)
			{
				for (int col = 0; col < 9; col++)
				{
					if (_board[row, col] != 0 &&
						_board[row, col] != _solutionBoard[row, col])
					{
						mistakes++;
					}
				}
			}

			return mistakes;
		}

		public bool IsCompleted()
		{
			bool completed = true;
			for (int row = 0; row < 9; row++)
			{
				for (int col = 0; col < 9; col++)
				{
					if (_board[row, col] != _solutionBoard[row, col])
					{
						completed = false;
						row = 9;
						col = 9;
					}
				}
			}

			return completed;
		}

		void Ctrl_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.X >= 0 && e.Y > HEADER_HEIGHT &&
				e.X < Size.Width && e.Y < Size.Height)
			{
				int boxSize = (this.Height - HEADER_HEIGHT) / 9;

				int row = (e.Y - HEADER_HEIGHT) / boxSize;
				int col = e.X / boxSize;

				// only allow player to put number on a cell that wasn't filled at the start.
				if (_initialBoard[row, col] == 0)
				{
					int digit = e.Button == MouseButtons.Right ? 0 : _selectedDigit;
					if (_mouseClickDel != null)
					{
						_mouseClickDel(row, col, digit);
					}
				}
			}
			else
			{
				int boxSize = (this.Height - HEADER_HEIGHT) / 9;
				if (e.X >= 0 && e.Y >= 0 &&
					e.X < Size.Width && e.Y < boxSize)
				{
					int col = e.X / boxSize;
					_selectedDigit = col + 1;
					Invalidate();
				}

			}
		}

		void Ctrl_MouseLeave(object sender, System.EventArgs e)
		{
			_highlightedSquare = new Point(-1, -1);
			Invalidate();
		}

		void Ctrl_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.X >= 0 && e.Y > HEADER_HEIGHT &&
				e.X < Size.Width && e.Y < Size.Height)
			{

				int boxSize = (this.Height - HEADER_HEIGHT) / 9;

				int row = (e.Y - HEADER_HEIGHT) / boxSize;
				int col = e.X / boxSize;

				_highlightedSquare = new Point(col, row);

				// Invalidate the control to draw the highlighted square.
				Invalidate();
			}
			else
			{
				_highlightedSquare = new Point(-1, -1);
				Invalidate();
			}
		}


		private void DrawText(Graphics g, Font font, string text, Rectangle rect)
		{
			StringFormat sf = new StringFormat
			{
				Alignment = StringAlignment.Center,
				LineAlignment = StringAlignment.Center
			};
			g.DrawString(text, font, Brushes.Black, rect, sf);
		}

		private void DrawNumbersOnBoard(Graphics g)
		{
			using Font fnt = new Font(this.Font.FontFamily, 16.0f);
			using Font fntBold = new Font(this.Font.FontFamily, 20.0f, FontStyle.Bold);
			int size = (this.Height - HEADER_HEIGHT) / 9;
			for (int row = 0; row < 9; row++)
			{
				for (int col = 0; col < 9; col++)
				{
					Rectangle rect = new Rectangle(new Point((col * size), (row * size) + HEADER_HEIGHT), new Size(size, size));
					if (_board[row, col] != 0)
					{
						bool partOfInitialSetup = _initialBoard[row, col] != 0;
						string num = _numbers.Substring(_board[row, col] - 1, 1);
						DrawText(g, partOfInitialSetup ? fntBold : fnt, num, rect);
					}
				}
			}
		}

		private void DrawHeader(Graphics g)
		{
			int boxSize = this.Size.Width / 9;

			using (Brush brush1 = new SolidBrush(Color.LightGray))
			{
				using Brush brushSelected = new SolidBrush(Color.Pink);
				using Pen framePen = new Pen(Color.Black, 1.0f);
				Rectangle rect = new Rectangle(new Point(0, 0), new Size(this.Size.Width - 1, boxSize - 1));
				g.FillRectangle(brush1, rect);

				if (_selectedDigit > 0)
				{
					Rectangle rectDigit = new Rectangle(new Point((_selectedDigit - 1) * boxSize, 0), new Size(boxSize - 1, boxSize - 1));
					g.FillRectangle(brushSelected, rectDigit);
				}

				g.DrawRectangle(framePen, rect);

				for (int col = 0; col < 8; col++)
				{
					int x = boxSize * (col + 1);
					g.DrawLine(framePen, x, 0, x, boxSize);
				}
			}
			DrawNumbersInHeader(g);
		}

		private void DrawNumbersInHeader(Graphics g)
		{
			using Font fnt = new System.Drawing.Font(this.Font.FontFamily, 16.0f);
			int size = this.Size.Width / 9;
			for (int col = 0; col < 9; col++)
			{
				Rectangle rect = new Rectangle(new Point((col * size), 0), new Size(size, size));
				string num = _numbers.Substring(col, 1);
				DrawText(g, fnt, num, rect);
			}
		}

		/// <summary>
		/// Paint the content of the control.
		/// </summary>
		/// <param name="e">A System.Windows.Forms.PaintEventArgs that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			// Call the OnPaint method of the base class.
			base.OnPaint(e);

			int size = (this.Height - HEADER_HEIGHT) / 9;
			int boardHeight = this.Height - HEADER_HEIGHT;

			using (Brush brush1 = new SolidBrush(Color.White),
					brush2 = new SolidBrush(Color.LightGoldenrodYellow),
					brush3 = new SolidBrush(Color.Lavender),
					brushError = new SolidBrush(Color.Red))
			{
				using Pen framePen = new Pen(Color.Black, 1.0f), thickerPen = new Pen(Color.Black, 3.0f);
				Rectangle rect = new Rectangle(new Point(0, HEADER_HEIGHT), new Size(this.Size.Width - 1, this.Size.Height - HEADER_HEIGHT - 1));
				e.Graphics.FillRectangle(brush1, rect);

				for (int row = 0; row < 9; row++)
				{
					for (int col = 0; col < 9; col++)
					{
						if (_initialBoard[row, col] != 0)
						{
							Rectangle rect2 = new Rectangle(new Point((col * size), (row * size) + HEADER_HEIGHT), new Size(size, size));
							e.Graphics.FillRectangle(brush2, rect2);
						}
						else
						{
							Rectangle rect2 = new Rectangle(new Point((col * size), (row * size) + HEADER_HEIGHT), new Size(size, size));
							if (InShowErrorsMode)
							{
								if (_board[row, col] != 0 &&
									_board[row, col] != _solutionBoard[row, col])
								{
									e.Graphics.FillRectangle(brushError, rect2);
									rect2.Inflate(-6, -6);
									e.Graphics.FillRectangle(brush1, rect2);
								}
							}
							else
							{
								if (_highlightedSquare.X == col && _highlightedSquare.Y == row)
								{
									e.Graphics.FillRectangle(brush3, rect2);
								}
							}
						}
					}
				}

				e.Graphics.DrawRectangle(framePen, rect);

				for (int row = 0; row < 8; row++)
				{
					int y = ((boardHeight / 9) * (row + 1)) + HEADER_HEIGHT;
					if (row % 3 == 2)
					{
						e.Graphics.DrawLine(thickerPen, 0, y - 1, this.Right, y - 1);
					}
					else
					{
						e.Graphics.DrawLine(framePen, 0, y, this.Right, y);
					}
				}
				for (int col = 0; col < 8; col++)
				{
					int x = (boardHeight / 9) * (col + 1);
					if (col % 3 == 2)
					{
						e.Graphics.DrawLine(thickerPen, x - 1, HEADER_HEIGHT, x - 1, this.Bottom);
					}
					else
					{
						e.Graphics.DrawLine(framePen, x, HEADER_HEIGHT, x, this.Bottom);
					}
				}
			}

			DrawNumbersOnBoard(e.Graphics);

			DrawHeader(e.Graphics);
		}
	}
}
