//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Umamido.DL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Slider
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Slider()
        {
            this.SliderDesc = new HashSet<SliderDesc>();
            this.SliderTitle = new HashSet<SliderTitle>();
        }
    
        public int SliderId { get; set; }
        public int ImageId { get; set; }
        public bool IsActive { get; set; }
        public string ButtonUrl { get; set; }
    
        public virtual Image Image { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SliderDesc> SliderDesc { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SliderTitle> SliderTitle { get; set; }
    }
}
