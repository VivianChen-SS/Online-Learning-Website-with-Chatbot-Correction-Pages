//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestGroup.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Unit
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Unit()
        {
            this.StudentTestGroup_Unit_Junction = new HashSet<StudentTestGroup_Unit_Junction>();
            this.ExtendQuestion = new HashSet<ExtendQuestion>();
        }
    
        public string UnitID { get; set; }
        public int Week { get; set; }
        public int Number { get; set; }
        public string VideoLink { get; set; }
        public string Question { get; set; }
        public byte[] QuestionImage { get; set; }
        public string QuestionImageLink { get; set; }
        public string Answer { get; set; }
        public byte[] ConceptMap { get; set; }
        public string ConceptMapLink { get; set; }
        public string ConceptMapDescription { get; set; }
        public string KeyWords { get; set; }
        public Nullable<System.DateTime> VideoStartDate { get; set; }
        public Nullable<System.DateTime> VideoEndDate { get; set; }
        public string VideoCoverImgLink { get; set; }
        public Nullable<double> VideoLength { get; set; }
        public Nullable<bool> MultipleChoice { get; set; }
        public string ChoiceText { get; set; }
        public string ChatbotAgentId { get; set; }
        public string ChatbotIntent { get; set; }
        public Nullable<int> ChoiceNumber { get; set; }
        public Nullable<System.DateTime> QuestionStartDateTime { get; set; }
        public Nullable<System.DateTime> QuestionEndDateTime { get; set; }
        public Nullable<System.DateTime> ConceptMapStartDate { get; set; }
        public Nullable<System.DateTime> ConceptMapEndDate { get; set; }
        public Nullable<bool> MultipleChatbot { get; set; }
        public string MultipleChatbotAgentID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentTestGroup_Unit_Junction> StudentTestGroup_Unit_Junction { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExtendQuestion> ExtendQuestion { get; set; }
    }
}
