using System;

using SignalBusNS;

public class MyMain{
    public static void Main(string[] args){
        int milliseconds = 50;

        var signalBus = new SignalBus();
        var rand = new Random(new Random().Next());

        Action<object> pendingAction = null;
        Action<object> processAction = null;
        Action<object> resultAction = null;

        int waitTime = 0;

        pendingAction = (_time)=>{
            var time = (int) _time;
            if(time <= 10){
                Clog($"Pending...{time}");
            }else{
                signalBus.Unsubscribe("pending", pendingAction);
                signalBus.Subscribe("process", processAction);
            }
        };
        processAction = (_time)=>{
            var time = (int) _time;
            if(time <= 20){
                Clog($"Processing...{time}");
                waitTime++;
            }else{
                signalBus.Unsubscribe("process", processAction);
                signalBus.Subscribe("result", resultAction);
            }
        };
        resultAction = (x)=>{
            Clog($"I waited {waitTime} milliseconds to get the bread!");
            signalBus.Unsubscribe("result", resultAction);
        };

        signalBus.Subscribe("pending", pendingAction);

        var time = 0;

        while(time<milliseconds){

            if(signalBus.HasSignal("pending")){
                signalBus.Emit("pending", time);
            }
            if(signalBus.HasSignal("process")){
                signalBus.Emit("process", time);
            }
            if(signalBus.IsSubscribed("result", resultAction)){
                signalBus.Emit("result");
            }

            var inc = rand.Next(1,5);
            time += inc;
        }

    }

    public static void Clog(object x){ Console.WriteLine(x);}

}