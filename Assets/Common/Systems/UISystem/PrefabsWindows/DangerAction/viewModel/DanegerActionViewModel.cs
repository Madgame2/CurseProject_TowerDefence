using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Common.systems.UI.Prefabs
{
    public class DangerActionViewModel
    {
        private TaskCompletionSource<bool> _tcs;
        public async Task<bool> GetResult()
        {
            _tcs = new TaskCompletionSource<bool>();
            return await _tcs.Task;
        }

        public void SetResult(bool result)
        {
            _tcs?.TrySetResult(result);
        }
    }
}
