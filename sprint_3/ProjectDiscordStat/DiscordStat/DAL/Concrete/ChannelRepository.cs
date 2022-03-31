﻿using System.Collections.Generic;
using System.Linq;
using DiscordStats.DAL.Abstract;
using DiscordStats.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.DAL.Concrete
{
    public class ChannelRepository : Repository<Channel>, IChannelRepository
    {
        public ChannelRepository(DiscordDataDbContext ctx) : base(ctx)
        {
        }
    }
}
