//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Monitor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class cow
    {
        public int id { get; set; }
        [Required]
        [Display(Name = "奶牛编号")]
        public string cowId { get; set; }
        [Display(Name = "年龄")]
        [DataType(DataType.Text)]
        public int age { get; set; }
        [Display(Name = "备注")]
        [DataType(DataType.MultilineText)]
        public string remarks { get; set; }
    }
}