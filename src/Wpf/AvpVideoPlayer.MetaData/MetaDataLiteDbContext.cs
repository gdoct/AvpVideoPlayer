﻿using LiteDB;

namespace AvpVideoPlayer.MetaData
{
    internal class MetaDataLiteDbContext : IDisposable, IMetaDataContext
    {
        private readonly LiteDatabase _db;

        internal MetaDataLiteDbContext(string databasefilename)
        {
            _db = new LiteDatabase(databasefilename);
            var col = _db.GetCollection<FileMetaData>("FileMetaData");
            col.EnsureIndex(x => new { x.Name, x.Length });
            col.EnsureIndex(x => x.Path);
        }

        public FileMetaData? GetMetadata(FileInfo file)
        {
            var col = _db.GetCollection<FileMetaData>("FileMetaData");
            var data = new { file.Name, Length = file.Exists ? file.Length : 0 };
            return col.Query()
                .Where(f => new { f.Name, f.Length } == new { data.Name, data.Length })
            .FirstOrDefault();
        }

        public void DeleteMetadata(FileMetaData metaData)
        {
            var col = _db.GetCollection<FileMetaData>("FileMetaData");
            var existing = col.Query()
                .Where(f => new { f.Name, f.Length } == new { metaData.Name, metaData.Length })
                .Where(f => f.Path == metaData.Path)
                .ToDocuments()
                .FirstOrDefault();

            if (existing != null)
            {
                col.Delete(existing["_id"]);
            }
        }

        public IEnumerable<FileMetaData> GetMetadata()
        {
            var col = _db.GetCollection<FileMetaData>("FileMetaData");
            return col.Query().ToEnumerable();
        }

        public IList<FileMetaData> GetMetadataForPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));
            var col = _db.GetCollection<FileMetaData>("FileMetaData");
            return col.Query()
                .Where(f => f.Path == path)
                .ToList();
        }

        public void SaveMetadata(FileMetaData metaData)
        {
            var col = _db.GetCollection<FileMetaData>("FileMetaData");
            var existing = col.Query()
                .Where(f => new { f.Name, f.Length } == new { metaData.Name, metaData.Length })
                .Where(f => f.Path == metaData.Path)
                .ToDocuments()
                .FirstOrDefault();

            if (existing != null)
            {
                var id = existing["_id"];
                col.Update(id, metaData);
            }
            else
            {
                col.Insert(metaData);
            }
        }

        public List<TagData> GetTags()
        {
            var col = _db.GetCollection<TagData>("Tags");
            return col.FindAll().ToList();
        }

        public void AddTag(string tag)
        {
            var col = _db.GetCollection<TagData>("Tags");
            var existing = col.Query()
                .Where(f => f.Name.ToUpper() == tag.ToUpper())
                .ToList();
            if (existing.Any()) return;
            col.Insert(new TagData { Name = tag });
        }

        public void DeleteTag(string tag)
        {
            var col = _db.GetCollection<TagData>("Tags");
            var existing = col.Query()
                .Where(f => f.Name.ToUpper() == tag.ToUpper())
                .ToDocuments()
                .FirstOrDefault();
            if (existing == null) return;
            col.Delete(existing["_id"]);
        }

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}