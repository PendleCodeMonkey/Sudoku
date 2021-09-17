using PendleCodeMonkey.SudokuLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PendleCodeMonkey.SudokuWinFormsApp
{
	public partial class SudokuForm : Form
	{
		#region fields

		private Sudoku _sudokuLib = null;


		private readonly int[] _board = new int[9 * 9];

		private bool _solved = false;

		private readonly List<string> _puzzles = new List<string>();

		private List<int[]> _history = new List<int[]>();

		private bool _helpMode = false;

		private CancellationTokenSource _cts;
		private bool _working = false;

		private System.Windows.Forms.Timer _cancButtonShowTimer = new System.Windows.Forms.Timer();

		#endregion

		public SudokuForm()
		{
			InitializeComponent();
		}

		private async Task<bool> GetSolution()
		{
			_working = true;
			EnableButtons();

			var timer = new Stopwatch();
			timer.Start();

			_cts = new CancellationTokenSource();
			Task<(bool, bool, string)> taskSolve = Task<(bool, bool, string)>.Factory.StartNew(() => _sudokuLib.Solve(_cts.Token), _cts.Token);

			bool cancelled = false;
			bool solved = false;
			bool valid = false;
			try
			{
				(valid, solved, _) = await taskSolve;
			}
			catch (OperationCanceledException)
			{
				cancelled = true;
			}
			finally
			{
				timer.Stop();
				_working = false;
				_cts.Dispose();
				_cts = null;
			}

			if (solved)
			{
				labelResult.Text = "[Solved in " + DurationToReadableString(timer.Elapsed) + "]";
			}
			else
			{
				labelResult.Text = cancelled ? "[CANCELLED - No solution found]" : valid ? "[No solution found]" : "[INVALID PUZZLE]";
			}

			return solved;
		}

		private string DurationToReadableString(TimeSpan span)
		{
			return string.Format("{0:0.0000} seconds.", span.TotalSeconds);
		}

		private async void button1_Click(object sender, EventArgs e)
		{
			await SetupBoardFromCurrentLayout();
		}

		private async Task SetupBoardFromCurrentLayout()
		{
			labelResult.Text = "";

			if (!CheckValidPuzzle(comboBoxPuzzles.Text))
			{
				labelResult.Text = "[INVALID PUZZLE]";
				return;
			}

			ClearHistory();

			if (comboBoxPuzzles.Text.Length > 0)
			{
				string s = comboBoxPuzzles.Text;
				_sudokuLib.InitializeBoard(s);
				_sudokuLib.Board.CopyTo(_board, 0);

				// Add initial board setup to the history.
				AddToHistory(_board);
			}

			panelContainer.SetBoard(_board, true);

			_solved = false;
			EnableButtons();

			labelResult.Text = "Solving....";

			// Start timer to show the "Cancel" button. this waits 1000ms before showing the button so that it does not appear
			// for puzzles that are solved very quickly (i.e that don't give the user anywhere near enough time to cancel)
			_cancButtonShowTimer.Interval = 1000;
			_cancButtonShowTimer.Start();

			_solved = await GetSolution();
			EnableButtons();
			if (_solved)
			{
				panelContainer.SetSolutionBoard(_sudokuLib.Solution);
			}
			_cancButtonShowTimer.Stop();
			btnCancelSolve.Visible = false;
		}

		private bool CheckValidPuzzle(string puzzle)
		{
			if (puzzle.Length == 0)
			{
				return false;
			}

			// Must contain only numbers and dash characters. Anything else is an invalid puzzle.
			int numDigits = 0;
			foreach (var c in puzzle)
			{
				if (char.IsDigit(c))
				{
					numDigits++;
				}
				else if (c != '-')
				{
					return false;
				}
			}

			// Must contain at least one digit to be valid.
			if (numDigits == 0)
			{
				return false;
			}

			return true;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			_sudokuLib = new Sudoku();

			// Resize the board to make its dimensions suitable (i.e. so that the squares are actually square)
			panelContainer.HEADER_HEIGHT = (panelContainer.Height / 10) + 16;
			int height = ((panelContainer.Height - panelContainer.HEADER_HEIGHT) / 9) * 9;
			panelContainer.Size = new Size(height, height + panelContainer.HEADER_HEIGHT);

			panelContainer.AttachEventHandlers(MouseClickDelegateHandler);

			labelResult.Text = "";

			ReadPuzzles("sudokus.txt");

			// If no puzzles were added from files then add a default hard-coded puzzle to work with.
			if (_puzzles.Count == 0)
			{
				_puzzles.Add("--7---9-8-3-17---4-----6---69874-3----3-1-4----1-39762---4-----9---51-4-4-5---1--");
			}

			foreach (var puz in _puzzles)
			{
				comboBoxPuzzles.Items.Add(puz);
			}

			panelContainer.Focus();

			// Cancel button is not initially visible.
			btnCancelSolve.Visible = false;

			_cancButtonShowTimer.Tick += new EventHandler(CancelButtonShowTimerEvent);

			EnableButtons();
		}

		private void CancelButtonShowTimerEvent(object sender, EventArgs e)
		{
			_cancButtonShowTimer.Stop();
			btnCancelSolve.Visible = true;
		}

		private void buttonShowSolution_Click(object sender, EventArgs e)
		{
			// If puzzle has been solved then show the solution.
			if (_solved)
			{
				_sudokuLib.Solution.CopyTo(_board, 0);
				panelContainer.SetBoard(_sudokuLib.Solution);
			}
		}

		private void ReadPuzzles(string filename)
		{
			try
			{
				string path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
				path += "\\" + filename;
				using StreamReader sr = new StreamReader(path);
				while (sr.Peek() >= 0)
				{
					string line = sr.ReadLine();
					line = line.Replace('0', '-');
					line = line.Replace('.', '-');
					_puzzles.Add(line);
				}
			}
			catch (Exception)
			{
			}
		}

		public void MouseClickDelegateHandler(int row, int col, int digit)
		{
			if (_helpMode)
			{
				if (_solved)
				{
					digit = _sudokuLib.Solution[(row * 9) + col];
				}
				ToggleHelpMode();
			}
			if (_board[row * 9 + col] != digit)
			{
				_board[row * 9 + col] = digit;

				panelContainer.SetBoard(_board);

				AddToHistory(_board);

				EnableButtons();

				if (panelContainer.IsCompleted())
				{
					labelResult.Text = "** PUZZLE COMPLETED **";
				}
			}
		}

		private void buttonClear_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 81; i++)
			{
				_board[i] = 0;
			}

			panelContainer.SetBoard(_board, true);
			comboBoxPuzzles.Text = "";

			labelResult.Text = "";

			ClearHistory();
			_solved = false;
			EnableButtons();
		}

		private void buttonGetString_Click(object sender, EventArgs e)
		{
			GetAsString();
		}

		private void GetAsString()
		{
			string strPuzzle = "";

			for (int row = 0; row < 9; ++row)
			{
				for (int col = 0; col < 9; ++col)
				{
					if (_board[row * 9 + col] > 0)
					{
						strPuzzle += (char)('0' + _board[row * 9 + col]);
					}
					else
					{
						strPuzzle += "-";
					}
				}
			}

			comboBoxPuzzles.Text = strPuzzle;
		}

		private void buttonUndo_Click(object sender, EventArgs e)
		{
			if (ClearHistoryAfterElement())
			{
				var board = _history[^1];
				board.CopyTo(_board, 0);

				panelContainer.SetBoard(_board);
			}

			EnableButtons();
		}

		private void ClearHistory()
		{
			if (_history == null)
			{
				_history = new List<int[]>();
			}
			else
			{
				_history.Clear();
			}
		}

		// Add the specified board layout to the history.
		private void AddToHistory(int[] board)
		{
			int[] board2 = new int[81];
			board.CopyTo(board2, 0);
			_history.Add(board2);
		}

		// Remove all elements in the history after the element at the specified index.
		// Note: removes only the last item from the history when index is -1 (the default value)
		private bool ClearHistoryAfterElement(int index = -1)
		{
			bool removed = false;
			if (_history.Count > 1)
			{
				int elem = (index == -1) ? _history.Count - 1 : index;
				if (elem >= 0 && elem < _history.Count)
				{
					_history.RemoveRange(elem, _history.Count - elem);
					removed = true;
				}
			}

			return removed;
		}

		// Rolls back (in the history) to the last board state that contains no errors.
		private bool RollbackToLastCorrectBoardState()
		{
			bool errorFound = true;
			int lastValidElem = -1;
			for (int elem = _history.Count - 1; elem >= 0 && errorFound; --elem)
			{
				errorFound = false;
				for (int i = 0; i < 81; i++)
				{
					if (_history[elem][i] > 0 &&
						_history[elem][i] != _sudokuLib.Solution[i])
					{
						errorFound = true;
						break;
					}
				}
				if (!errorFound)
				{
					lastValidElem = elem;
				}
			}

			bool rolledBack = false;
			if (lastValidElem >= 0)
			{
				ClearHistoryAfterElement(lastValidElem + 1);
				rolledBack = true;
			}

			return rolledBack;
		}

		private void buttonRollback_Click(object sender, EventArgs e)
		{
			if (RollbackToLastCorrectBoardState())
			{
				_history[^1].CopyTo(_board, 0);

				panelContainer.SetBoard(_board);

				EnableButtons();
			}

		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			ToggleHelpMode();
		}

		private void ToggleHelpMode()
		{
			_helpMode = !_helpMode;
			Cursor = _helpMode ? Cursors.Help : Cursors.Default;
		}

		private void EnableButtons()
		{
			buttonRollback.Enabled = !_working && _solved;
			buttonUndo.Enabled = _history.Count > 1;
			buttonHelp.Enabled = _solved;
			buttonShowSolution.Enabled = _solved;
			buttonShow.Enabled = !_working;
			buttonClear.Enabled = !_working;
			buttonGetString.Enabled = !_working;
			labelShowErrors.Enabled = _solved;
		}

		private void labelShowErrors_MouseDown(object sender, MouseEventArgs e)
		{
			panelContainer.InShowErrorsMode = true;
			panelContainer.Invalidate();

			EnableButtons();
		}

		private void labelShowErrors_MouseUp(object sender, MouseEventArgs e)
		{
			panelContainer.InShowErrorsMode = false;
			panelContainer.Invalidate();
		}

		private void btnCancelSolve_Click(object sender, EventArgs e)
		{
			if (_working)
			{
				_cts?.Cancel();
			}
		}
	}
}
