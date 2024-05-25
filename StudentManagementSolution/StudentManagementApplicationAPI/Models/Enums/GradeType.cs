using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplicationAPI.Models.Enums
{
    public enum GradeType
    {
        [Description(("91 - 100"))] O = 10,
        [Description(("81 - 90"))] A_Plus = 9, 
        [Description(("71 - 80"))] A = 8,  
        [Description(("61 - 70"))] B_Plus = 7, 
        [Description(("51 - 60"))] B = 6, 
        [Description(("40 - 50 "))] C = 5, 
        [Description(("Below 40"))] F = 4, 
        [Description(("UnAttempted"))] UA = -1, 
        [Description(("Re-Attempted"))] RA = -1
    }
}
