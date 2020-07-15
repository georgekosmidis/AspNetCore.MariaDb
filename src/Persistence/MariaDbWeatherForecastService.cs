using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.MariaDB.Persistence
{
    public sealed class MariaDbWeatherForecastService : IMariaDbWeatherForecastService
    {
        private readonly MariaDbContext _dbContext;

        public MariaDbWeatherForecastService(MariaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Delete(int id)
        {
            try
            {
                _dbContext.WeatherForecasts.Remove(
                    new WeatherForecastDataModel
                    {
                        Id = id
                    }
                );

                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return 0;
            }
        }

        public async Task<IEnumerable<WeatherForecastDataModel>> FindAll()
        {
            return await _dbContext.WeatherForecasts.ToListAsync();
        }

        public async Task<WeatherForecastDataModel> FindOne(int id)
        {
            return await _dbContext.WeatherForecasts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> Insert(WeatherForecastDataModel forecast)
        {
            _dbContext.Add(forecast);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> Update(WeatherForecastDataModel forecast)
        {
            try
            {
                _dbContext.Update(forecast);
                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return 0;
            }
        }
    }

}

