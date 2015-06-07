PLUGIN.Title = "PaiN TimedExecute"
PLUGIN.Description = "Execute commands every (x) seconds."
PLUGIN.Author = "PaiN"
PLUGIN.Version = V(1, 5, 0)
PLUGIN.ResourceId = 919

function PLUGIN:LoadDefaultConfig()
    self.Config.ShowTimedCommands = "true" 
	self.Config.TimerOnceCommands = { 
{"say 'Restart in 1 MINUTE!'",60}, 
{"say 'Restart in 30 Seconds!'",90}, 
{"restart",120},
{"reset.oncetimer",121},
}
    self.Config.RepeatTimedCommands = { 
{"server.save",300},
{"say 'hello world'",5},
}
self:SaveConfig()
end
 
function PLUGIN:OnServerInitialized()
    self:RepeaterTimedCommands()
	self:OnceTimedCommands()
end

function PLUGIN:Init()
command.AddConsoleCommand("reset.oncetimer", self.Plugin, "cmdResetOnceTimer")
command.AddConsoleCommand("reset.repeatertimer", self.Plugin, "cmdResetRepeaterTimer")
permission.RegisterPermission("canrestimers", self.Plugin)
     self.timers = {}
	 self.timer = {}
end

function PLUGIN:Unload()
self:ResetTimers()
end

function PLUGIN:cmdResetOnceTimer()
self:OnceTimedCommands()
end
function PLUGIN:cmdResetRepeaterTimer()
self:RepeaterTimedCommands()
end

function PLUGIN:ResetTimers()
for k,v in pairs(self.timers) do
self.timers[k]:Destroy()
end
for k,v in pairs(self.timer) do
self.timer[k]:Destroy()
	end
end

function PLUGIN:RepeaterTimedCommands()
    self:ResetTimers()
for k,v in pairs(self.Config.RepeatTimedCommands) do 
self.timers[k] = timer.Repeat(v[2], 0, function()
    print("[" .. self.Title .. "] Ran command: " .. v[1])
        rust.RunServerCommand(v[1])
		end, self.Plugin)
		end
	end
	
function PLUGIN:OnceTimedCommands()
	self:ResetTimers()
for k,v in pairs(self.Config.TimerOnceCommands) do 
self.timer[k] = timer.Once(v[2], function ()
	print("[" .. self.Title .. "] Ran command: " .. v[1])
        rust.RunServerCommand(v[1])
    end, self.Plugin)
	end
end
