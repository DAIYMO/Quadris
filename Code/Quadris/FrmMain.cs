using Quadris.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace Quadris
{
    public partial class FrmMain : Form
    {
        private const int BOARD_COLS = 10;
        private const int BOARD_ROWS = 20;

        private const int CELL_WIDTH = 20;
        private const int CELL_HEIGHT = 20;

        private const int HELD_BOARD_COLS = 6;
        private const int HELD_BOARD_ROWS = 6;


        private Label[,] gridControls;
        private Label[,] heldGridControls;
        private Board board;
        private HeldBoard heldBoard;

        private SoundPlayer sndPlayer;

        private static readonly Dictionary<PieceColor, Image> pieceColorToImgMap = new Dictionary<PieceColor, Image> {
      {PieceColor.BLUE, Resources.cell_blue},
      {PieceColor.CYAN, Resources.cell_cyan},
      {PieceColor.GREEN, Resources.cell_green},
      {PieceColor.MAGENTA, Resources.cell_magenta},
      {PieceColor.ORANGE, Resources.cell_orange},
      {PieceColor.PURPLE, Resources.cell_purple},
      {PieceColor.RED, Resources.cell_red},
      {PieceColor.WHITE, Resources.cell_white},
      {PieceColor.YELLOW, Resources.cell_yellow},
      {PieceColor.FIRE, Resources.cell_fire},
      {PieceColor.ICE, Resources.cell_ice},
      {PieceColor.GROUND, Resources.cell_ground},
      {PieceColor.DARK, Resources.cell_dark},
    };

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            board = new Board();
            heldBoard = new HeldBoard();
            Piece piece = Piece.GetRandPiece();
            board.ActivePiece = piece;

            CreateGrid();
            CreateHoldGrid();
            //sndPlayer = new SoundPlayer(Resources.bg_music);
            //sndPlayer.PlayLooping();
        }

        private void CreateGrid()
        {
            panBoard.Width = CELL_WIDTH * BOARD_COLS + 4;
            panBoard.Height = CELL_HEIGHT * BOARD_ROWS + 4;
            gridControls = new Label[BOARD_ROWS, BOARD_COLS];
            panBoard.Controls.Clear();
            for (int col = 0; col < BOARD_COLS; col++)
            {
                for (int row = 0; row < BOARD_ROWS; row++)
                {
                    Label lblCell = MakeGridCell(row, col);
                    panBoard.Controls.Add(lblCell);
                    gridControls[row, col] = lblCell;
                }
            }
        }
        private void CreateHoldGrid()
        {
            holdPanel.Width = CELL_WIDTH * HELD_BOARD_COLS + 4;
            holdPanel.Height = CELL_HEIGHT * HELD_BOARD_ROWS + 4;
            heldGridControls = new Label[HELD_BOARD_ROWS, HELD_BOARD_COLS];
            holdPanel.Controls.Clear();
            for (int col = 0; col < HELD_BOARD_COLS; col++)
            {
                for (int row = 0; row < HELD_BOARD_ROWS; row++)
                {
                    Label lblCell = MakeGridCell(row, col);
                    holdPanel.Controls.Add(lblCell);
                    heldGridControls[row, col] = lblCell;
                }
            }
        }

        private void UpdateGrid()
        {
            for (int col = 0; col < BOARD_COLS; col++)
            {
                for (int row = 0; row < BOARD_ROWS; row++)
                {
                    GridCellInfo cellInfo = board.Grid[row + 4, col];
                    if (cellInfo.State == CellState.OCCUPIED_ACTIVE_PIECE || cellInfo.State == CellState.OCCUPIED_PREVIOUSLY)
                    {
                        gridControls[row, col].Image = pieceColorToImgMap[cellInfo.Color];
                    }
                    else
                    {
                        gridControls[row, col].Image = null;
                    }
                }
            }
        }

        private void UpdateHeldGrid()
        {

            for (int col = 0; col < HELD_BOARD_COLS - 2; col++)
            {
                for (int row = 0; row < HELD_BOARD_ROWS - 2; row++)
                {
                    //GridCellInfo heldCellInfo = heldBoard.HeldGrid[row, col];
                    if(heldBoard.heldPiece == null)
                    {
                        heldGridControls[row + 1, col + 1].Image = null;
                        continue;
                    }
                    if (heldBoard.heldPiece.Layout[row, col] == true)
                    {
                        heldGridControls[row + 1 , col + 1].Image = pieceColorToImgMap[heldBoard.heldPiece.Color];
                    }
                    else
                    {
                        heldGridControls[row + 1 , col + 1].Image = null;
                    }
                }
            }    
        }

        private Label MakeGridCell(int row, int col)
        {
            return new Label()
            {
                Text = "",
                Width = CELL_WIDTH,
                Height = CELL_HEIGHT,
                FlatStyle = FlatStyle.Flat,
                Top = row * CELL_HEIGHT,
                Left = col * CELL_WIDTH
            };
        }

        private void tmrFps_Tick(object sender, EventArgs e)
        {
            board.Update();
            UpdateGrid();
            UpdateHeldGrid();
        }

        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.X:
                    board.RotateActivePieceRight();
                    break;
                case Keys.Z:
                    board.RotateActivePieceLeft();
                    break;
                case Keys.Right:
                    board.MoveActivePieceRight();
                    break;
                case Keys.Left:
                    board.MoveActivePieceLeft();
                    break;
                case Keys.H:
                    heldBoard.getCurrentActivePiece(board);
                    heldBoard.updateHeldGrid();
                    board.ActivePiece = Piece.GetRandPiece();
                    break;
                case Keys.G:
                    board.ActivePiece = heldBoard.heldPiece;
                    heldBoard.heldPiece = null;
                    break; 
            }
        }
    }
}
