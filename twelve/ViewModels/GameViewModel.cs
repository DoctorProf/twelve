using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using twelve.Data;
using twelve.Models;
using twelve.ViewModels.Base;


namespace twelve.ViewModels
{
    internal class GameViewModel : ViewModel
    {
        private FieldModel selectedField = null;
        public FieldModel SelectedField { get => selectedField; set => Set(ref selectedField, value); }
        private Color.Piece move = Color.Piece.White;
        public Color.Piece Move { get => move; set => Set(ref move, value); }

        #region Field Observable Collection
        private ObservableCollection<ObservableCollection<FieldModel>> f = new();
        public ObservableCollection<ObservableCollection<FieldModel>> F { get => f; set => Set(ref f, value); }
        #endregion

        #region Constructor
        public GameViewModel()
        {
            StartGame();
        }
        #endregion

        #region Methods

        #region StartGame
        public void StartGame()
        {
            Move = Color.Piece.White;
            GenerateField();
            StartingPosition();
        }
        #endregion
        #region SetPointCheck
        public void SetPointCheck(int checkposi, int checkposj)
        {
            if (CheckOnBoard(checkposi, checkposj))
            {
                if (SelectedField.PieceColor == Color.Piece.White)
                {
                    if (F[checkposi][checkposj].TexturePath == ImagesConstsPaths.Empty)
                    {
                        SetPoint(checkposi, checkposj);
                    }
                }
                if (SelectedField.PieceColor == Color.Piece.Black)
                {
                    if (F[checkposi][checkposj].TexturePath == ImagesConstsPaths.Empty)
                    {
                        SetPoint(checkposi, checkposj);
                    }
                }
            }
        }
        public void Kill(Color.Piece color)
        {
            for (int i = 1; i <= 4; i++)
            {
                int checkposi = SelectedField.I - 1;
                int checkposj = SelectedField.J;

                if (CheckOnBoard(checkposi, checkposj))
                {

                    if (CheckOnBoard(checkposi - 1, checkposj) && F[checkposi][checkposj].PieceColor != color && F[checkposi][checkposj].PieceColor != Color.Piece.Empty)
                    {
                        SetPoint(checkposi - 1, checkposj);
                    }
                }
            }
            for (int i = 1; i <= 4; i++)
            {
                int checkposi = SelectedField.I + 1;
                int checkposj = SelectedField.J;

                if (CheckOnBoard(checkposi, checkposj))
                {

                    if (CheckOnBoard(checkposi + 1, checkposj) && F[checkposi][checkposj].PieceColor != color && F[checkposi][checkposj].PieceColor != Color.Piece.Empty)
                    {
                        SetPoint(checkposi + 1, checkposj);
                    }
                }
            }
            for (int i = 1; i <= 4; i++)
            {
                int checkposi = SelectedField.I;
                int checkposj = SelectedField.J - 1;

                if (CheckOnBoard(checkposi, checkposj))
                {

                    if (CheckOnBoard(checkposi, checkposj - 1) && F[checkposi][checkposj].PieceColor != color && F[checkposi][checkposj].PieceColor != Color.Piece.Empty)
                    {
                        SetPoint(checkposi, checkposj - 1);
                    }
                }
            }
            for (int i = 1; i <= 4; i++)
            {
                int checkposi = SelectedField.I;
                int checkposj = SelectedField.J + 1;

                if (CheckOnBoard(checkposi, checkposj))
                {

                    if (CheckOnBoard(checkposi, checkposj + 1) && F[checkposi][checkposj].PieceColor != color && F[checkposi][checkposj].PieceColor != Color.Piece.Empty)
                    {
                        SetPoint(checkposi, checkposj + 1);
                    }
                }
            }
        }
        #region Walk
        public void Walk()
        {
            int checkposi = SelectedField.I;
            int checkposi1 = SelectedField.I + 1;
            int checkposi2 = SelectedField.I - 1;

            int checkposj = SelectedField.J;
            int checkposj1 = SelectedField.J + 1;
            int checkposj2 = SelectedField.J - 1;

            SetPointCheck(checkposi, checkposj1);
            SetPointCheck(checkposi, checkposj2);
            SetPointCheck(checkposi1, checkposj);
            SetPointCheck(checkposi2, checkposj);


        }
        #endregion

        #endregion
        #region CheckOnBoard
        public static bool CheckOnBoard(int i, int j)
        {
            return i >= 0 && i < 5 && j >= 0 && j < 5;
        }
        #endregion

        #region Quit

        public static void Quit() { Application.Current.Shutdown(); }

        #endregion

        #region RestartGame

        public void RestartGame(string text)
        {
            MessageBoxResult mbr = MessageBox.Show($"{text} Начать заново?", "Конец игры", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes) StartGame();
            else Quit();
        }

        #endregion

        #region Win

        public void Win(Color.Piece color)
        {
            string colorName = (color == Color.Piece.White) ? "белые" : "черные";
            RestartGame($"Выйграли {colorName}.");
        }

        #endregion

        #region CheckWin
        public void CheckWin()
        {
            int countWhite = 0;
            int countBlack = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (F[i][j].PieceColor == Color.Piece.White)
                    {
                        countWhite++;
                    }
                    if (F[i][j].PieceColor == Color.Piece.Black)
                    {
                        countBlack++;
                    }
                }
            }
            if (countWhite == 0)
            {
                Win(Color.Piece.Black);
            }
            if (countBlack == 0)
            {
                Win(Color.Piece.White);
            }
            if(countWhite == 2 && countBlack == 2) 
            {
                RestartGame("Ничья");
            }
        }
        #endregion

        public void ReverseFigures(FieldModel field)
        {
            if(field.I == SelectedField.I - 2 && field.J == SelectedField.J)
            {
                F[field.I + 1][field.J].PieceColor = Color.Piece.Empty;
            }
            if (field.I == SelectedField.I + 2 && field.J == SelectedField.J)
            {
                F[field.I - 1][field.J].PieceColor = Color.Piece.Empty;
            }
            if (field.I == SelectedField.I && field.J == SelectedField.J - 2)
            {
                F[field.I][field.J + 1].PieceColor = Color.Piece.Empty;
            }
            if (field.I == SelectedField.I && field.J == SelectedField.J + 2)
            {
                F[field.I][field.J - 1].PieceColor = Color.Piece.Empty;
            }
            field.PieceColor = SelectedField.PieceColor;
            SelectedField.PieceColor = Color.Piece.Empty;
            SelectedField.Selected = false;
            SelectedField = null;
            ClearPoints();
            Move = Move == Color.Piece.White ? Color.Piece.Black : Color.Piece.White;
            CheckWin();
        }
        public void SetPoint(int i, int j)
        {
            if (CheckOnBoard(i, j) && F[i][j].TexturePath == ImagesConstsPaths.Empty)
            {
                F[i][j].TexturePath = ImagesConstsPaths.Point;
            }
        }
        public void ClearPoints()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (F[i][j].TexturePath == ImagesConstsPaths.Point)
                    {
                        F[i][j].TexturePath = ImagesConstsPaths.Empty;
                    }
                }
            }
        }
        public void Click(FieldModel field)
        {
            if (field.TexturePath != ImagesConstsPaths.Empty && field.PieceColor == Move)
            {
                if (field.PieceColor == Move)
                {

                    if (SelectedField == null)
                    {
                        SelectedField = field;
                        SelectedField.Selected = true;
                        Walk();
                        Kill(SelectedField.PieceColor);
                    }
                    else
                    {
                        if (field == SelectedField)
                        {
                            SelectedField.Selected = false;
                            SelectedField = null;
                            ClearPoints();

                        }
                        else if (field != SelectedField)
                        {
                            SelectedField.Selected = false;
                            ClearPoints();
                            SelectedField = field;
                            SelectedField.Selected = true;
                            Walk();
                            Kill(SelectedField.PieceColor);
                        }
                    }
                }
            }
            else
            {
                if (SelectedField != null)
                {
                    if (field.TexturePath == ImagesConstsPaths.Point)
                    {
                        ReverseFigures(field);
                    }
                }
            }
        }

        #region GenerateField
        public void GenerateField()
        {
            f.Clear();
            for (int i = 0; i < 5; i++)
            {
                ObservableCollection<FieldModel> row = new();
                for (int j = 0; j < 5; j++)
                {
                    row.Add(new FieldModel() { I = i, J = j, BackgroundColor = "#24a7ed" });
                }
                f.Add(row);
            }
        }
        #endregion

        #endregion
        #region Starting position
        public void StartingPosition()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (i < 2 || (i == 2 && j < 2))
                    {
                        F[i][j].PieceColor = Color.Piece.Black;
                    }
                    else
                    {
                        F[i][j].PieceColor = Color.Piece.White;
                    }
                    if (i == 2 && j == 2)
                    {
                        f[i][j].PieceColor = Color.Piece.Empty;
                    }
                }
            }
        }

        #endregion
    }
}
