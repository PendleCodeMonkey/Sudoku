using PendleCodeMonkey.SudokuLib;
using System;

namespace PendleCodeMonkey.SudokuConsoleApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Sudoku sudoku = new Sudoku();

			string puzzle = "-96-4--3--5782----1--9--5----9-1---85-------24---9-6----4--3--1----7926--2--5-98-";
			sudoku.InitializeBoard(puzzle);

			Console.WriteLine("PUZZLE:");
			OutputPuzzle(puzzle);
			Console.WriteLine();

			var (valid, solved, solution) = sudoku.Solve();
			if (!valid)
			{
				Console.WriteLine("INVALID PUZZLE");
			}
			else
			{
				if (solved)
				{
					Console.WriteLine("SOLVED:");
					OutputPuzzle(solution);
				}
				else
				{
					Console.WriteLine("NO SOLUTION FOUND");
				}
			}

			Console.ReadKey();
		}

		private static void OutputPuzzle(string puzzle)
		{
			Console.WriteLine("-------------");
			for (int row = 0; row < 9; row++)
			{
				Console.Write("|");
				for (int col = 0; col < 9; col++)
				{
					char c = puzzle[row * 9 + col];
					Console.Write(c == '.' ? ' ' : c);
					if (col % 3 == 2)
					{
						Console.Write("|");
					}
				}
				Console.WriteLine();
				if (row % 3 == 2)
				{
					Console.WriteLine("-------------");
				}
			}
		}
	}
}
