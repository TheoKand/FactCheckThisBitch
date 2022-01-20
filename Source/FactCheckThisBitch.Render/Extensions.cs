using FactCheckThisBitch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FactCheckThisBitch.Render
{
    public static class Extensions
    {
        public static string GetKeywordsText(this Piece piece,int skip=0,int take=100)
        {
            var result = string.Join(Environment.NewLine,
                piece.Keywords.Skip(skip).Take(take).Select(k => $"{k.KeywordFormat()}"));
            return result;
        }

        private static string KeywordFormat(this string keyword)
        {
            keyword = keyword.Trim();
            if (keyword.Length > 13)
            {
                return keyword.Replace(" ",
                    Environment.NewLine);
            }

            return keyword;
        }

        public static dynamic GetPieceCoordinates(this Puzzle puzzle,
            int pieceIndex)
        {
            switch (pieceIndex)
            {
                case 1:
                    return new
                    {
                        X = 17,
                        Y = 11,
                        KeywordsBoxes = new dynamic[]
                        {
                            new
                            {
                                Left = 0,
                                Top = 0,
                                Right = 250,
                                Bottom = 157
                            }
                        }
                    };
                case 2:
                    return new
                    {
                        X = 280,
                        Y = 11,
                        KeywordsBoxes = new dynamic[]
                        {
                            new
                            {
                                Left = 0,
                                Top = 0,
                                Right = 250,
                                Bottom = 80
                            }
                        }

                    };
                case 3:
                    return new
                    {
                        X = 449,
                        Y = 11,
                        KeywordsBoxes = new dynamic[]
                        {
                            new
                            {
                                Left = 95,
                                Top = 0,
                                Right = 344,
                                Bottom = 156
                            }
                        }

                    };
                case 4:
                    return new
                    {
                        X = 17,
                        Y = 182,
                        KeywordsBoxes = new dynamic[]
                        {
                            new
                            {
                                Left = 0,
                                Top = 96,
                                Right = 253,
                                Bottom = 174
                            },
                            new
                            {
                                Left = 0,
                                Top = 274,
                                Right = 253,
                                Bottom = 349
                            }
                        }

                    };
                case 5:
                    return new
                    {
                        X = 186,
                        Y = 181,
                        KeywordsBoxes = new dynamic[]
                        {
                            new
                            {
                                Left = 99,
                                Top = 99,
                                Right = 344,
                                Bottom = 346
                            }
                        }

                    };
                case 6:
                    return new
                    {
                        X = 544,
                        Y = 181,
                        KeywordsBoxes = new dynamic[]
                        {
                            new
                            {
                                Left = 0,
                                Top = 94,
                                Right = 253,
                                Bottom = 174
                            },
                            new
                            {
                                Left = 0,
                                Top = 267,
                                Right = 253,
                                Bottom = 352
                            }
                        }

                    };
                case 7:
                    return new
                    {
                        X = 18,
                        Y = 543,
                        KeywordsBoxes = new dynamic[]
                        {
                            new
                            {
                                Left = 2,
                                Top = 98,
                                Right = 246,
                                Bottom = 250
                            }
                        }

                    };
                case 8:
                    return new
                    {
                        X = 280,
                        Y = 543,
                        KeywordsBoxes = new dynamic[]
                        {
                            new
                            {
                                Left = 3,
                                Top = 176,
                                Right = 248,
                                Bottom = 255
                            }
                        }
                    };
                case 9:
                    return new
                    {
                        X = 449,
                        Y = 543,
                        KeywordsBoxes = new dynamic[]
                        {
                            new
                            {
                                Left = 97,
                                Top = 97,
                                Right = 342,
                                Bottom = 248
                            }
                        }

                    };
                default:
                    return new
                    {
                        X = 50,
                        Y = 50,
                        KeywordsBoxes = new dynamic[]
                        {
                            new
                            {
                                Left = 0,
                                Top = 0,
                                Right = 0,
                                Bottom = 0
                            }
                        }
                    };
            }
        }
    }
}