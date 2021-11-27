using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FactCheckThisBitch.Models
{
    public static class Extensions
    {
        public static Type ToPieceContentType(this PieceType pieceType)
        {
            var enumValue = pieceType.ToString();
            Type type = Assembly.GetExecutingAssembly().GetType($"FactCheckThisBitch.Models.{enumValue}");
            return type;
        }

        public static BaseContent ToPieceContent(this PieceType pieceType)
        {
            var enumValue = pieceType.ToString();
            Type type = Assembly.GetExecutingAssembly().GetType($"FactCheckThisBitch.Models.{enumValue}");  
            return  Activator.CreateInstance(type) as BaseContent;
        }

        public static PieceType ToPieceType(this BaseContent pieceContent)
        {
            var enumValue = pieceContent.GetType().Name;
            return (PieceType)Enum.Parse(typeof(PieceType), enumValue);
        }
    }
}
