using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplicationAPI.Models.Enums
{
    public enum GradeType
    {
        [Description(("91 - 100"))] O,
        [Description(("81 - 90"))] A_Plus, 
        [Description(("71 - 80"))] A,  
        [Description(("61 - 70"))] B_Plus, 
        [Description(("51 - 60"))] B, 
        [Description(("40 - 50 "))] C, 
        [Description(("Below 40"))] F, 
        [Description(("UnAttempted"))] UA, 
        [Description(("Re-Attempted"))] RA
    }
}
