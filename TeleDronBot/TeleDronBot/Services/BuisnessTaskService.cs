﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.DTO;

namespace TeleDronBot.Services
{
    class BuisnessTaskService : RepositoryProvider
    {
        public async ValueTask<int> CountTask() =>
            await buisnessTaskRepository.Get().CountAsync();

        public async Task Update(BuisnessTaskDTO task)
        {
            if (task == null)
                throw new System.Exception("task cannot be null");
            await buisnessTaskRepository.Update(task);
        }

        public async ValueTask<BuisnessTaskDTO> GetCurrentTask(long chatid)
        {
            ShowOrdersDTO order = await showOrdersRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
            return await buisnessTaskRepository.Get().FirstOrDefaultAsync(i => i.Id == order.CurrentProductId);
        }

        public async ValueTask<int> CountTask(long chatid) =>
            await buisnessTaskRepository.Get().Where(i => i.ChatId == chatid).CountAsync();

        public async ValueTask<BuisnessTaskDTO> GetFirstElement() =>
            await buisnessTaskRepository.Get().FirstOrDefaultAsync();

        public async ValueTask<BuisnessTaskDTO> GetFirstElement(long chatid) =>
            await buisnessTaskRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);

        public async ValueTask<BuisnessTaskDTO> FindTaskByTaskId(int id) =>
            await buisnessTaskRepository.Get().FirstOrDefaultAsync(i => i.Id == id);


        public async ValueTask<BuisnessTaskDTO> FindTask(long chatid)
        {
            return await buisnessTaskRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
        }
        
        public async Task Create(BuisnessTaskDTO task)
        {
            if (task.ChatId == 0 || task.Region == null)
                throw new System.Exception("Incorrect data");
            await buisnessTaskRepository.Create(task);
        }
        
        public async ValueTask<bool> IsUserBuisnessman(long chatid)
        {
            UserDTO user = await userRepository.FindById(chatid);
            
            if (user == null)
                return false;

            if (user.BusinessPrivilage == 0)
                return false;
            return true;
        }
    }
}
