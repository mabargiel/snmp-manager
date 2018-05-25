using System;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Data;
using Microsoft.EntityFrameworkCore;
using SNMPManager.Core.Entities;
using SNMPManager.Core.Sonars.Exceptions;

namespace SNMPManager.Core.Sonars.Repositories
{
    public class SonarsRepository : IRepository<Sonar>, IDisposable
    {
        private readonly SnmpManagerContext _context;
        private bool _disposed;

        public SonarsRepository(SnmpManagerContext context)
        {
            _context = context;
        }

        public IQueryable<Sonar> GetAllSonars()
        {
            return _context.Sonars.AsQueryable();
        }

        public Sonar GetSonar(Guid id)
        {
            return _context.Sonars.Find(id);
        }

        public void CreateSonar(Sonar sonar)
        {
            _context.Sonars.Add(sonar);
        }

        public void UpdateSonar(Sonar sonar)
        {
            var toUpdate = _context.Sonars.Find(sonar.Id);

            if (toUpdate == null)
                throw new SonarMissingException($"Sonar with id {sonar.Id} does not exist");

            _context.Entry(toUpdate).State = EntityState.Detached;
            _context.Sonars.Attach(sonar);
            _context.Entry(sonar).State = EntityState.Modified;
        }

        public void RemoveSonar(Guid id)
        {
            var toRemove = _context.Sonars.Find(id);

            if (toRemove == null)
                throw new SonarMissingException($"Sonar with id {id} does not exist");

            _context.Remove(toRemove);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
