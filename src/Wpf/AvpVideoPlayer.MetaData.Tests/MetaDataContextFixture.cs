using System;
using System.IO;

namespace AvpVideoPlayer.MetaData.Tests
{
    public class MetaDataContextFixture : IDisposable
    {
        private readonly string _dbFilename;
        private readonly MetaDataLiteDbContext _context;
        private bool disposedValue;

        internal MetaDataLiteDbContext Context => _context;

        public MetaDataContextFixture()
        {
            _dbFilename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                $"avpmeta-test-{Guid.NewGuid()}.dat");
            _context = new MetaDataLiteDbContext(_dbFilename);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                    File.Delete(_dbFilename);
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
