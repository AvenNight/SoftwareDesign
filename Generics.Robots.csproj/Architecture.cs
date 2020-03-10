using System.Collections.Generic;

namespace Generics.Robots
{
    public interface IRobotAI<out TCommand>
    {
        TCommand GetCommand();
    }

    public class ShooterAI : IRobotAI<IMoveCommand>
    {
        int counter = 1;
        public IMoveCommand GetCommand() => ShooterCommand.ForCounter(counter++);
    }

    public class BuilderAI : IRobotAI<IMoveCommand>
    {
        int counter = 1;
        public IMoveCommand GetCommand() => BuilderCommand.ForCounter(counter++);
    }

    public interface IDevice<in TCommand>
    {
        string ExecuteCommand(TCommand command);
    }

    public class Mover : IDevice<IMoveCommand>
    {
        public string ExecuteCommand(IMoveCommand command) =>
            $"MOV {command.Destination.X}, {command.Destination.Y}";
    }

    public class Robot
    {
        IRobotAI<IMoveCommand> ai;
        IDevice<IMoveCommand> device;

        public Robot(IRobotAI<IMoveCommand> ai, IDevice<IMoveCommand> executor)
        {
            this.ai = ai;
            this.device = executor;
        }

        public IEnumerable<string> Start(int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                var command = ai.GetCommand();
                if (command == null)
                    break;
                yield return device.ExecuteCommand(command);
            }
        }

        public static Robot Create(IRobotAI<IMoveCommand> ai, IDevice<IMoveCommand> executor) =>
            new Robot(ai, executor);
    }
}