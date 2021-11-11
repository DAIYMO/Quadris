using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quadris
{
    public class HeldBoard
    {
        public GridCellInfo[,] HeldGrid { get; private set; }
        public Piece heldPiece { get; set; }

        public HeldBoard()
        {
            HeldGrid = new GridCellInfo[6, 6];
            for (int i = 0; i < HeldGrid.GetLength(0); i++)
            {
                for (int j = 0; j < HeldGrid.GetLength(1); j++)
                {
                    HeldGrid[i, j] = new GridCellInfo();
                }
            }
        }
        public void getCurrentActivePiece(Board b)
        {
            Piece tempPiece = b.ActivePiece;
            heldPiece = Piece.MakePiece(tempPiece.Type);
            
        }

        private void updateWithHeldPiece()
        {
            for (int r = 0; r < HeldGrid.GetLength(0); r++)
            {
                for (int c = 0; c < HeldGrid.GetLength(1); c++)
                {
                    if(heldPiece == null)
                    {
                        GridCellInfo cellInfo = HeldGrid[r, c];
                        cellInfo.Color = PieceColor.NONE;
                        cellInfo.State = CellState.EMPTY;
                    }
                }
            }
            for (int r = 0; r < heldPiece.Layout.GetLength(0); r++)
            {
                for (int c = 0; c < heldPiece.Layout.GetLength(1); c++)
                {
                    if (heldPiece.Layout[r, c])
                    {
                        GridCellInfo cellInfo = GetCellInfo(heldPiece.GridRow, heldPiece.GridCol);
                        cellInfo?.SetToActivePiece(heldPiece);
                    }
                }
            }
  
        }

        public GridCellInfo GetCellInfo(int row, int col)
        {
            if (row < 0 || row >= HeldGrid.GetLength(0) || col < 0 || col >= HeldGrid.GetLength(1))
                return null;
            else
                return HeldGrid[row, col];
        }

        public void updateHeldGrid()
        {
            updateWithHeldPiece();
        }
    }
}
