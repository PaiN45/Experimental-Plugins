PLUGIN.Title = "TimedExecute"
PLUGIN.Version = V(3, 0, 0)
PLUGIN.Description = "Executes a command every (x) seconds."
PLUGIN.Author = "PaiN"
PLUGIN.HasConfig = true
PLUGIN.ResourceId = 919

function PLUGIN:LoadDefaultConfig()
    self.Config.ShowTimedCommands = "true"
    self.Config.TimedCommands = { 
{"server.save",300},
{"say 'hello world'",5},
}
end

function PLUGIN:OnServerInitialized()
    self:TimedCommands()
end

function PLUGIN:Init()
     self.timers = {}
end

function PLUGIN:Unload()
self:ResetTimers()
end

function PLUGIN:ResetTimers()
for k,v in pairs(self.timers) do
self.timers[k]:Destroy()
end
end



function PLUGIN:TimedCommands()
    self:ResetTimers()
for k,v in pairs(self.Config.TimedCommands) do 
self.timers[k] = timer.Repeat(v[2], 0, function()
    print("[" .. self.Title .. "] Ran command: " .. v[1])
        rust.RunServerCommand(v[1])
    end, self.Plugin)
end
end