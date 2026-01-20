using CarpetCleaningSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Domain.Entities
{
    public class Carpet
    {
        public int CarpetLabelNumber { get; private set; }
        public decimal Width { get; private set; }
        public decimal Length { get; private set; }
        public CarpetMaterial Material { get; private set; }
        public Boolean IsLocked { get; private set; } = false;

        public decimal Surface => Width * Length;

        private Carpet() { } // For ORM tools

        private Carpet(int carpetLabelNumber, decimal width, decimal length, CarpetMaterial material)
        {
            if (carpetLabelNumber > 0) CarpetLabelNumber = carpetLabelNumber;
            else throw new ArgumentException("A carpetLabelNumber must be greater than 0.", nameof(carpetLabelNumber));

            if (width > 0) Width = width;
            else throw new ArgumentException("Width must be greater than 0.", nameof(width));

            if (length > 0) Length = length;
            else throw new ArgumentException("Length must be greater than 0.", nameof(length));

            if (!Enum.IsDefined(typeof(CarpetMaterial), material))
                throw new ArgumentException("Invalid carpet material.", nameof(material));

            Material = material;

            IsLocked = false;
        }

        public static Carpet CreateCarpet(int carpetLabelNumber, decimal width, decimal length, CarpetMaterial material)
        {
            return new Carpet(carpetLabelNumber, width, length, material);
        }


        public void ChangeDimensions(decimal newWidth, decimal newLength)
        {
            if (IsLocked)
                throw new InvalidOperationException("Cannot change dimensions of a locked carpet.");

            if (newWidth <= 0) throw new ArgumentException("Width must be greater than 0.", nameof(newWidth));

            if (newLength <= 0) throw new ArgumentException("Length must be greater than 0.", nameof(newLength));

            if (Length == newLength && Width == newWidth) return;


            Width = newWidth;
            Length = newLength;
        }

        public void ChangeMaterial(CarpetMaterial newMaterial)
        {
            if(IsLocked)
                throw new InvalidOperationException("Cannot change material of a locked carpet.");

            if (!Enum.IsDefined(typeof(CarpetMaterial), newMaterial))
                throw new ArgumentException("Invalid carpet material.", nameof(newMaterial));

            if (Material == newMaterial) return;

            Material = newMaterial;
        }

        public void Lock()
        {
            if (IsLocked) return;
            IsLocked = true;
        }

        public void Unlock()
        {
            if (!IsLocked) return;
            IsLocked = false;
        }
    }
}
