namespace PendleCodeMonkey.SudokuWinFormsApp
{
	partial class SudokuForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.buttonShow = new System.Windows.Forms.Button();
			this.buttonShowSolution = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.comboBoxPuzzles = new System.Windows.Forms.ComboBox();
			this.labelResult = new System.Windows.Forms.Label();
			this.buttonClear = new System.Windows.Forms.Button();
			this.buttonGetString = new System.Windows.Forms.Button();
			this.buttonUndo = new System.Windows.Forms.Button();
			this.buttonRollback = new System.Windows.Forms.Button();
			this.buttonHelp = new System.Windows.Forms.Button();
			this.labelShowErrors = new System.Windows.Forms.Label();
			this.panelContainer = new PendleCodeMonkey.SudokuWinFormsApp.SudokuBoardCtrl();
			this.btnCancelSolve = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// buttonShow
			// 
			this.buttonShow.Location = new System.Drawing.Point(665, 5);
			this.buttonShow.Name = "buttonShow";
			this.buttonShow.Size = new System.Drawing.Size(102, 33);
			this.buttonShow.TabIndex = 0;
			this.buttonShow.Text = "SHOW";
			this.buttonShow.UseVisualStyleBackColor = true;
			this.buttonShow.Click += new System.EventHandler(this.button1_Click);
			// 
			// buttonShowSolution
			// 
			this.buttonShowSolution.BackColor = System.Drawing.SystemColors.Info;
			this.buttonShowSolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.buttonShowSolution.Location = new System.Drawing.Point(605, 498);
			this.buttonShowSolution.Name = "buttonShowSolution";
			this.buttonShowSolution.Size = new System.Drawing.Size(163, 49);
			this.buttonShowSolution.TabIndex = 2;
			this.buttonShowSolution.Text = "Show Solution";
			this.buttonShowSolution.UseVisualStyleBackColor = false;
			this.buttonShowSolution.Click += new System.EventHandler(this.buttonShowSolution_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label1.Location = new System.Drawing.Point(10, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(61, 17);
			this.label1.TabIndex = 3;
			this.label1.Text = "Puzzle:";
			// 
			// comboBoxPuzzles
			// 
			this.comboBoxPuzzles.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.comboBoxPuzzles.FormattingEnabled = true;
			this.comboBoxPuzzles.Location = new System.Drawing.Point(86, 9);
			this.comboBoxPuzzles.Name = "comboBoxPuzzles";
			this.comboBoxPuzzles.Size = new System.Drawing.Size(573, 25);
			this.comboBoxPuzzles.TabIndex = 5;
			// 
			// labelResult
			// 
			this.labelResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.labelResult.Location = new System.Drawing.Point(466, 50);
			this.labelResult.Name = "labelResult";
			this.labelResult.Size = new System.Drawing.Size(296, 20);
			this.labelResult.TabIndex = 6;
			this.labelResult.Text = "label2";
			this.labelResult.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// buttonClear
			// 
			this.buttonClear.Location = new System.Drawing.Point(640, 198);
			this.buttonClear.Name = "buttonClear";
			this.buttonClear.Size = new System.Drawing.Size(123, 38);
			this.buttonClear.TabIndex = 8;
			this.buttonClear.Text = "Clear Board";
			this.buttonClear.UseVisualStyleBackColor = true;
			this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
			// 
			// buttonGetString
			// 
			this.buttonGetString.Location = new System.Drawing.Point(640, 248);
			this.buttonGetString.Name = "buttonGetString";
			this.buttonGetString.Size = new System.Drawing.Size(123, 38);
			this.buttonGetString.TabIndex = 9;
			this.buttonGetString.Text = "Get As String";
			this.buttonGetString.UseVisualStyleBackColor = true;
			this.buttonGetString.Click += new System.EventHandler(this.buttonGetString_Click);
			// 
			// buttonUndo
			// 
			this.buttonUndo.Location = new System.Drawing.Point(640, 296);
			this.buttonUndo.Name = "buttonUndo";
			this.buttonUndo.Size = new System.Drawing.Size(123, 38);
			this.buttonUndo.TabIndex = 10;
			this.buttonUndo.Text = "UNDO";
			this.buttonUndo.UseVisualStyleBackColor = true;
			this.buttonUndo.Click += new System.EventHandler(this.buttonUndo_Click);
			// 
			// buttonRollback
			// 
			this.buttonRollback.Location = new System.Drawing.Point(640, 398);
			this.buttonRollback.Name = "buttonRollback";
			this.buttonRollback.Size = new System.Drawing.Size(123, 38);
			this.buttonRollback.TabIndex = 11;
			this.buttonRollback.Text = "Rollback";
			this.buttonRollback.UseVisualStyleBackColor = true;
			this.buttonRollback.Click += new System.EventHandler(this.buttonRollback_Click);
			// 
			// buttonHelp
			// 
			this.buttonHelp.Location = new System.Drawing.Point(640, 447);
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.Size = new System.Drawing.Size(123, 38);
			this.buttonHelp.TabIndex = 12;
			this.buttonHelp.Text = "HELP";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
			// 
			// labelShowErrors
			// 
			this.labelShowErrors.AutoSize = true;
			this.labelShowErrors.BackColor = System.Drawing.Color.Red;
			this.labelShowErrors.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.labelShowErrors.ForeColor = System.Drawing.Color.White;
			this.labelShowErrors.Location = new System.Drawing.Point(640, 370);
			this.labelShowErrors.Name = "labelShowErrors";
			this.labelShowErrors.Size = new System.Drawing.Size(122, 17);
			this.labelShowErrors.TabIndex = 13;
			this.labelShowErrors.Text = "Highlight Mistakes";
			this.labelShowErrors.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelShowErrors_MouseDown);
			this.labelShowErrors.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labelShowErrors_MouseUp);
			// 
			// panelContainer
			// 
			this.panelContainer.InShowErrorsMode = false;
			this.panelContainer.Location = new System.Drawing.Point(10, 77);
			this.panelContainer.Name = "panelContainer";
			this.panelContainer.Size = new System.Drawing.Size(428, 470);
			this.panelContainer.TabIndex = 1;
			this.panelContainer.TabStop = true;
			// 
			// btnCancelSolve
			// 
			this.btnCancelSolve.Location = new System.Drawing.Point(688, 81);
			this.btnCancelSolve.Name = "btnCancelSolve";
			this.btnCancelSolve.Size = new System.Drawing.Size(73, 27);
			this.btnCancelSolve.TabIndex = 14;
			this.btnCancelSolve.Text = "Cancel";
			this.btnCancelSolve.UseVisualStyleBackColor = true;
			this.btnCancelSolve.Click += new System.EventHandler(this.btnCancelSolve_Click);
			// 
			// SudokuForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(778, 558);
			this.Controls.Add(this.btnCancelSolve);
			this.Controls.Add(this.labelShowErrors);
			this.Controls.Add(this.buttonHelp);
			this.Controls.Add(this.buttonRollback);
			this.Controls.Add(this.buttonUndo);
			this.Controls.Add(this.buttonGetString);
			this.Controls.Add(this.buttonClear);
			this.Controls.Add(this.labelResult);
			this.Controls.Add(this.comboBoxPuzzles);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonShowSolution);
			this.Controls.Add(this.panelContainer);
			this.Controls.Add(this.buttonShow);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SudokuForm";
			this.Text = "Sudoku Solver";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonShow;
		private SudokuBoardCtrl panelContainer;
		private System.Windows.Forms.Button buttonShowSolution;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBoxPuzzles;
		private System.Windows.Forms.Label labelResult;
		private System.Windows.Forms.Button buttonClear;
		private System.Windows.Forms.Button buttonGetString;
		private System.Windows.Forms.Button buttonUndo;
		private System.Windows.Forms.Button buttonRollback;
		private System.Windows.Forms.Button buttonHelp;
		private System.Windows.Forms.Label labelShowErrors;
		private System.Windows.Forms.Button btnCancelSolve;
	}
}

