using DAM.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM.Domain.InfraEntities
{
    public sealed class Blob
    {
        public BlobItemKind Kind { get; set; }

        public bool IsFolder => Kind == BlobItemKind.Folder;

        public bool IsFile => Kind == BlobItemKind.File;

        public string? FolderPath { get; private set; }

        public string? Name { get; private set; }

        public long? Size { get; set; }

        public DateTimeOffset? CreatedTime { get; set; }

        public DateTimeOffset? LastModificationTime { get; set; }
    }
}
