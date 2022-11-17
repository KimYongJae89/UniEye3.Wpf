using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Data.Library
{
    public class Template
    {
        public float FileVersion { get; set; }
        public List<string> TagList { get; set; } = new List<string>();
        public Target Target { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        #region Properties

        public Guid Id { get; set; }

        public FileInfo FileInfo { get; set; }

        public float Width => Target.BaseRegion.Width;

        public float Height => Target.BaseRegion.Height;

        public string Key => Id.ToString();//this.FileInfo.Name + Id;

        #endregion


        public Template()
        {

        }

        public Template(string name)
        {
            Name = name;
        }

        public void GrantGuid()
        {
            if (Id == Guid.Empty)
            {
                Id = Guid.NewGuid();
            }
        }

        public Target CloneTarget()
        {
            var cloneTarget = (Target)Target.Clone();
            cloneTarget.Template = this;

            return cloneTarget;
        }

        public void SetTag(string tag)
        {
            string[] tagToken = tag.Split(';');
            TagList.AddRange(tagToken);
        }

        public string GetTag()
        {
            string tag = "";
            TagList.ForEach(x => tag += x + ";");

            return tag.TrimEnd(';');
        }

        public void Rename(string name)
        {
            // 파일명 변경 등... 
        }

        public void Save()
        {
            StepModelWriter modelWriter = StepModelWriterBuilder.Create();
            modelWriter.Write(this, Path);
        }

        public void Load(FileInfo fileInfo)
        {
            var modelReader = new StepModelReader();
            modelReader.Load(this, fileInfo.FullName);
        }
    }
}
