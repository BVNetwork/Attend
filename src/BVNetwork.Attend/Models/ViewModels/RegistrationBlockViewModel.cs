using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Core;
using BVNetwork.Attend.Models.Blocks;

namespace BVNetwork.Attend.Models.ViewModels
{
    public class RegistrationBlockViewModel<T> where T : BlockData
    {
        public RegistrationBlock CurrentBlock;

        public RegistrationBlockViewModel(T currentBlock) : base()
        {
            CurrentBlock = currentBlock as RegistrationBlock;

        }
    }
}