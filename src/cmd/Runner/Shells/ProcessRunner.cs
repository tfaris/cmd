using System;
using System.Diagnostics;
using cmd.Commands;
using cmd.Runner.Arguments;

namespace cmd.Runner.Shells
{
    internal class ProcessRunner : IRunner
    {
        private readonly Lazy<IArgumentBuilder> argumentBuilder = new Lazy<IArgumentBuilder>(() => new ArgumentBuilder());

        protected virtual IArgumentBuilder ArgumentBuilder
        {
            get { return argumentBuilder.Value; }
        }

        public string BuildArgument(Argument argument)
        {
            return ArgumentBuilder.Build(argument);
        }

        public virtual ICommando GetCommand()
        {
            return new Commando(this);
        }

        public virtual string Run(IRunOptions runOptions)
        {
            var process = new Process
                        {
                            StartInfo =
                                {
                                    FileName = runOptions.Command,
                                    Arguments = runOptions.Arguments,
                                    UseShellExecute = false,
                                    RedirectStandardOutput = true,
                                    RedirectStandardError = true
                                }
                        };

            System.Text.StringBuilder output = new System.Text.StringBuilder();
            process.Start();
            output.Append(process.StandardOutput.ReadToEnd());
            output.Append(process.StandardError.ReadToEnd());
            process.WaitForExit();
            return output.ToString();
        }
    }
}
