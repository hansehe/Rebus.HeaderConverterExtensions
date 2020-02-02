using System;
using Rebus.Config;
using Rebus.Pipeline;
using Rebus.Pipeline.Receive;
using Rebus.Pipeline.Send;

namespace Rebus.HeaderConverterExtensions.Utilities
{
    public static class OptionsConfigurerExtensions
    {       
        public static OptionsConfigurer RegisterIncomingStep<T>(this OptionsConfigurer configurer, 
            T stepToInject, 
            PipelineRelativePosition position = PipelineRelativePosition.Before, 
            Type anchorStep = null) where T : IIncomingStep
        {
            if (configurer.Has<T>())
            {
                return configurer;
            }
            
            anchorStep = anchorStep ?? typeof(DispatchIncomingMessageStep);
            configurer.Decorate<IPipeline>(c =>
            {
                var pipeline = c.Get<IPipeline>();

                return new PipelineStepInjector(pipeline)
                    .OnReceive(stepToInject, position, anchorStep);
            });
            configurer.Register(context => stepToInject);

            return configurer;
        }
        
        public static OptionsConfigurer RegisterOutgoingStep<T>(this OptionsConfigurer configurer, 
            T stepToInject, 
            PipelineRelativePosition position = PipelineRelativePosition.Before, 
            Type anchorStep = null) where T : IOutgoingStep
        {
            if (configurer.Has<T>())
            {
                return configurer;
            }
            
            anchorStep = anchorStep ?? typeof(SendOutgoingMessageStep);
            configurer.Decorate<IPipeline>(c =>
            {
                var pipeline = c.Get<IPipeline>();

                return new PipelineStepInjector(pipeline)
                    .OnSend(stepToInject, position, anchorStep);
            });
            configurer.Register(context => stepToInject);

            return configurer;
        }
    }
}