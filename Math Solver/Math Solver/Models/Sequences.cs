using SQLite;

namespace Math_Solver.Models
{
    public class Sequences
    {
        public int FormulaId { get; set; }
        public string Sequence { get; set; }
        public string Lines { get; set; }
        public string IdExtraFormula { get; set; }
    }
}
