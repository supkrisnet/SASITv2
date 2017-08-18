using System.Linq;
using System.Windows.Forms;

namespace Utiles.Forms
{
    public class Helper
    {
        public static Control FindByTag(Control root, string tag)
        {
            if (root == null)
            {
                return null;
            }

            if (root.Tag is string && (string)root.Tag == tag)
            {
                return root;
            }

            return (from Control control in root.Controls select FindByTag(control, tag)).FirstOrDefault(c => c != null);
        }
    }
}
