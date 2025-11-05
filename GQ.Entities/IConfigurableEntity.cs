using Microsoft.EntityFrameworkCore;

namespace GQ.Entities;

public interface IConfigurableEntity
{
    void OnModelCreating( ModelBuilder m );
}