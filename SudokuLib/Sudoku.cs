using System;
using System.Linq;
using System.Text;

namespace PendleCodeMonkey.SudokuLib
{
	/// <summary>
	/// Implementation of the <see cref="Sudoku"/> class.
	/// </summary>
	public class Sudoku
	{

		// The initial puzzle layout.
		private int[] _board = new int[81];

		// The solution is stored here when done.
		private readonly int[] _solution = new int[81];

		// Indicates if the puzzle has been solved.
		private bool _solved;

		// All the groups (nine rows, nine columns, and nine 3x3 grids - making 27 groups in total, each containing nine square locations).
		private readonly int[,] _groupSquareLocations = new int[27, 9];

		// Locations of all the squares that could contain conflicts for each square (20 locations for each of the 81 squares on the board).
		private readonly int[,] _conflictScanLocations = new int[81, 20];

		private int _iterationCount = 0;

		private System.Threading.CancellationToken _cancToken;

		public int[] Board => _board;
		public int[] Solution => _solution;

		public int IterationCount => _iterationCount;

		/// <summary>
		/// Initializes a new instance of the <see cref="Sudoku"/> class.
		/// </summary>
		public Sudoku()
		{
			InitializeData();
		}

		/// <summary>
		/// Initialize the board using the supplied layout string.
		/// </summary>
		/// <param name="layout">A string containing the layout of the board.</param>
		public void InitializeBoard(string layout)
		{
			int square = 0;
			if (!string.IsNullOrEmpty(layout))
			{
				int numSquares = Math.Min(layout.Length, 81);
				while (square < numSquares)
				{
					char c = layout[square];
					_board[square] = char.IsDigit(c) ? c - '0' : 0;
					square++;
				}
			}

			while (square < 81)
			{
				_board[square] = 0;
				square++;
			}

			_solved = false;
		}

		/// <summary>
		/// Solve the current Sudoku puzzle.
		/// </summary>
		/// <param name="cancToken">Cancellation token allowing the caller to cancel the operation.</param>
		/// <returns>A tuple containing the following values -
		///   valid - A boolean value indicating if the puzzle is valid.
		///   solved - A boolean value indicating if the puzzle was solved.
		///   solution - A string containing the solution to the puzzle if successfully solved, otherwise an empty string.</returns>
		public (bool valid, bool solved, string solution) Solve(System.Threading.CancellationToken cancToken = default)
		{
			_cancToken = cancToken;

			int[] workingBoard = new int[81];
			_board.CopyTo(workingBoard, 0);
			int numClues = workingBoard.Where(elem => elem > 0).Count();

			_solved = false;
			bool valid = false;
			string solutionString = string.Empty;
			if (IsBoardValid(workingBoard))
			{
				_iterationCount = 0;
				Solve(workingBoard, numClues);

				StringBuilder sb = new StringBuilder();
				foreach (var elem in _solution)
				{
					sb.Append(elem.ToString());
				}
				solutionString = sb.ToString();
				valid = true;
			}

			return (valid, _solved, solutionString);
		}


		/// <summary>
		/// Initialize data that is used by the Sudoku solving functionality.
		/// </summary>
		private void InitializeData()
		{
			// Determine the group square locations (i.e. populate the arrays that contain the locations of the squares in each row, column, and 3x3 box)
			int group = 0;

			// All nine Rows
			for (int row = 0; row < 9; row++)
			{
				for (int col = 0; col < 9; col++)
				{
					_groupSquareLocations[group, col] = row * 9 + col;
				}
				group++;
			}

			// All nine Columns
			for (int col = 0; col < 9; col++)
			{
				for (int row = 0; row < 9; row++)
				{
					_groupSquareLocations[group, row] = row * 9 + col;
				}
				group++;
			}

			// All nine 3x3 box groups
			for (int boxRow = 0; boxRow < 3; boxRow++)
			{
				for (int boxCol = 0; boxCol < 3; boxCol++)
				{
					int idx = 0;
					for (int row = 3 * boxRow; row < 3 * boxRow + 3; row++)
					{
						for (int col = 3 * boxCol; col < 3 * boxCol + 3; col++)
						{
							_groupSquareLocations[group, idx++] = row * 9 + col;
						}
					}
					group++;
				}
			}

			// Determine the conflict scan locations for each of the 81 squares on the board.
			// There are 20 locations stored for each square on the board (i.e. 8 for the other squares on the same row, 8 for the other squares on
			// the same column, and 4 for the other squares that occupy the remainder of the 3x3 box that the square is in)
			int squareIndex = 0;
			for (int row = 0; row < 9; row++)
			{
				for (int col = 0; col < 9; col++)
				{
					int boxRow = 3 * (row / 3);
					int boxCol = 3 * (col / 3);
					int elementIdx = 0;

					// Get locations of the other squares in the 3x3 box that contains this square.
					for (int r = boxRow; r < boxRow + 3; r++)
					{
						for (int c = boxCol; c < boxCol + 3; c++)
						{
							if (r != row || c != col)
							{
								_conflictScanLocations[squareIndex, elementIdx++] = r * 9 + c;
							}
						}
					}

					// Get locations of the other squares in the same column as this square (but only those that have not already been
					// handled because they are also in the same 3x3 box group)
					for (int r = 0; r < 9; r++)
					{
						if (r < boxRow || r >= boxRow + 3)
						{
							_conflictScanLocations[squareIndex, elementIdx++] = r * 9 + col;
						}
					}

					// Get locations of the other squares in the same row as this square (but only those that have not already been
					// handled because they are also in the same 3x3 box group)
					for (int c = 0; c < 9; c++)
					{
						if (c < boxCol || c >= boxCol + 3)
						{
							_conflictScanLocations[squareIndex, elementIdx++] = row * 9 + c;
						}
					}
					squareIndex++;
				}
			}
		}

		/// <summary>
		/// Determines if the specified board is valid (i.e. that there aren't duplicate
		/// numbers on a row, column, or in a 3x3 group)
		/// </summary>
		/// <param name="board">The state of the board to be checked.</param>
		/// <returns><c>true</c> if the board is valid, otherwise <c>false</c>.</returns>
		private bool IsBoardValid(int[] board)
		{
			bool duplicateFound = false;
			for (int row = 0; row < 9 && !duplicateFound; row++)
			{
				for (int col = 0; col < 9 && !duplicateFound; col++)
				{
					int digit = board[(row * 9) + col];
					if (digit != 0)
					{
						int squareIndex = (row * 9) + col;
						for (int m = 0; m < 20 && !duplicateFound; m++)
						{
							duplicateFound = (board[_conflictScanLocations[squareIndex, m]] == digit);
						}
					}
				}
			}
			return !duplicateFound;
		}

		/// <summary>
		/// Attempts to solve the Sudoku puzzle.
		/// </summary>
		/// <remarks>
		/// This is a recursive method.
		/// </remarks>
		/// <param name="board">The current state of the board.</param>
		/// <param name="level">The current level of solution (the number of digits that have been filled in so far).</param>
		private void Solve(int[] board, int level)
		{
			int leastNumOptions = 10;
			int[] bestOptionPositions = new int[9];
			int digitWithLeastOptions = 0;

			if (!_solved)
			{
				_iterationCount++;

				// Throw OperationCanceledException if a cancellation has been requested.
				_cancToken.ThrowIfCancellationRequested();

				// If level is 81 then we have filled in all of the squares (and therefore the puzzle has been solved)
				if (level == 81)
				{
					// Copy the board state into the solution array.
					board.CopyTo(_solution, 0);

					// and flag the puzzle as solved.
					_solved = true;
				}
				else
				{
					bool[] filled = new bool[10];
					int[] squareOptions = new int[9];

					// Check all groups (i.e. all nine rows, all nine columns, and all nine 3x3 box groups)
					for (int group = 0; group < 27; group++)
					{
						// First check which numbers have not yet been filled in.
						for (int digit = 1; digit < 10; digit++)
						{
							filled[digit] = false;
						}
						for (int i = 0; i < 9; i++)
						{
							filled[board[_groupSquareLocations[group, i]]] = true;
						}

						for (int digit = 1; digit <= 9; digit++)
						{
							// We're only interested in digits that have not already been filled in.
							if (!filled[digit])
							{
								int numOptions = 0;
								int groupElementIndex = 0;

								// Test all empty squares in the current group (i.e. row / column / 3x3 box); however, we
								// stop if we end up with more options that our current best (i.e. least number of options).
								while (groupElementIndex < 9 && numOptions < leastNumOptions) // Try all empty squares
								{
									int squareIndex = _groupSquareLocations[group, groupElementIndex];
									if (board[squareIndex] == 0)
									{
										// This is an unoccupied square.
										// Check each of the "conflict scan" squares (i.e. all squares
										// in the same row, column, and 3x3 box group)
										bool found = false;
										for (int i = 0; i < 20 && !found; i++)
										{
											found = found || board[_conflictScanLocations[squareIndex, i]] == digit;
										}

										if (!found)
										{
											// This digit wasn't found in any of the "conflict scan" squares
											// so log this square as a possible location for the digit.
											squareOptions[numOptions++] = squareIndex;
										}
									}
									groupElementIndex++;
								}

								// Check if there are any valid options and if the number of options
								// for this digit are fewer than our previous best (i.e. is this our new best?).
								if (numOptions > 0 && numOptions < leastNumOptions)
								{
									for (int i = 0; i < numOptions; i++)
									{
										bestOptionPositions[i] = squareOptions[i];
									}
									leastNumOptions = numOptions;
									digitWithLeastOptions = digit;
								}
								else if (numOptions == 0)   // We've found no options for this digit so bail out right here.
								{
									group = 27;
									break;
								}
							}
						}
					}
					if (leastNumOptions < 10) // Is it possible to continue?
					{
						// Try all positions for the best digit we've found.
						for (int i = 0; i < leastNumOptions; i++)
						{
							board[bestOptionPositions[i]] = digitWithLeastOptions;
							Solve(board, level + 1);
							board[bestOptionPositions[i]] = 0;  // Clear the square (for backtracking).
						}
					}
				}
			}
		}
	}
}
