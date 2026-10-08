using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Tests.Shared
{
    public static class DbSetMockFactory
    {
        public static Mock<DbSet<T>> Create<T>(List<T> entityList, Func<T, Guid> getId) where T : class
        {
            Mock<DbSet<T>> set = entityList.BuildMockDbSet();
            set.Setup(s => s.FindAsync(It.IsAny<object[]>())).ReturnsAsync((object[] keys) => entityList.FirstOrDefault(x => getId(x) == (Guid)keys[0]));
            return set;
        }
    }
}
