using Erd_Tools;
using PropertyHook;
using PvPHelper.Core;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Threading;
using System;

namespace PvPHelper.Console.Commands
{
    internal class TestModal : CommandBase
    {
        private PHPointer PhantomParamID;
        private ErdHook hook;
        private DispatcherTimer timer;
        public TestModal(ErdHook hook)
        {
            CommandString = "/test";
            RequireParams = true;
            HasParams = true;
            this.hook = hook;

            PhantomParamID = hook.CreateChildPointer(hook.WorldChrMan, new int[] { 0x1E508});

            timer = new();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += Timer_Tick;
        }
        private int tickStartValue;
        private int tickEndValue;
        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (tickStartValue < tickEndValue)
            {
                PhantomParamID.WriteInt32(0x538, tickStartValue);
                CommandManager.Log(tickStartValue.ToString());
                tickStartValue++;
            }
            else
            {
                timer.Stop();
                CommandManager.Log("End of Timer");
            }
        }

        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (int.TryParse(parameters[0], out int baseValue) && int.TryParse(parameters[1], out int maxValue))
            {
                tickStartValue = baseValue;
                tickEndValue = maxValue;
                timer.Start();
            }
        }
    }
}
