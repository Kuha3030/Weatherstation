//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVC_Sasema_test.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class searches
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public searches()
        {
            this.data = new HashSet<data>();
        }
    
        public int search_id { get; set; }
        public string hash_id { get; set; }
        public Nullable<System.DateTime> timestamp { get; set; }
        public string input_location { get; set; }
        public Nullable<System.DateTime> expires_yrno { get; set; }
        public Nullable<System.DateTime> last_modified_yrno { get; set; }
        public Nullable<System.DateTime> expires_FMI { get; set; }
        public Nullable<System.DateTime> last_modified_FMI { get; set; }
        public Nullable<System.DateTime> expires_foreca { get; set; }
        public Nullable<System.DateTime> last_modified_foreca { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<data> data { get; set; }
    }
}
