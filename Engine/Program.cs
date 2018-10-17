﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions;

namespace Engine
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            // get engine config
            IEngineConfig engineConfig = EngineConfigProvider.GetConfig();
            ILogger logger = EngineLoggerProvider.GetLogger(engineConfig);

            try
            {
                // Create objects for current environment
                ITestE2EReader e2eReader = TestE2EReaderProvider.GetReader(logger);
                IComputer computer = ComputerSelector.GetCurrentComputer();
                IReadOnlyList<ICloudOCRService> imageService = ICloudOCRServiceProvider.GetCloudOCRServices(computer, engineConfig, logger);

                // create engine
                IEngine engine = new Engine(e2eReader, computer, imageService, engineConfig, logger);

                // run tests
                await engine.RunAsync(args).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.WriteError("Error = " + ex);
            }
        }
    }
}
