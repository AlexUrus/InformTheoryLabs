using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR_1.Models
{
    public class HammingCodeModel
    {
        private MatrixManager matrixManager;
        public HammingCodeModel()
        {
            matrixManager = new MatrixManager();
        }
        public string GetHammingEncode(string message)
        {
            var bytes = matrixManager.GetCodeConstructionsFromCipher(message);
            var sb = new StringBuilder();
            foreach (var c in bytes)
            {
                foreach (var item in c)
                {
                    sb.Append(item);
                }
            }
            return sb.ToString();
        }
    }
}
