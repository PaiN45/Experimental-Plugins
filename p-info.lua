PLUGIN.Title        = "PaiN Info"
PLUGIN.Description  = "Check your target's info and yours."
PLUGIN.Author       = "PaiN"
PLUGIN.Version      = V(2, 0, 0)

local ChatMute
local AFKSystem

function PLUGIN:Init()
    command.AddChatCommand("myinfo",  self.Plugin, "cmdMyInfo")
	command.AddChatCommand("ainfo", self.Plugin, "cmdAinfo")
	command.AddChatCommand("info", self.Plugin, "cmdInfo")
	permission.RegisterPermission("cancheckinfo", self.Plugin)
	afkData = datafile.GetDataTable("afksystem")
	self:LoadDefaultConfig()
end
function PLUGIN:OnServerInitialized()
	if not plugins.Exists("chatmute") then
            print("*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*")
			print("[PaiN Info]: Chat Mute is not installed! You can find it at http://oxidemod.org/plugins/chatmute.1053/")
			print("*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*")
		return
	end
	if not plugins.Exists("p-afk") then
            print("*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*")
			print("[PaiN Info]: AFK System is not installed! You can find it at http://oxidemod.org/plugins/pain-afk.976/")
			print("*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*^*")
		return
	end
    ChatMute = plugins.Find("chatmute")
	AFKSystem = plugins.Find("p-afk")
end
 function PLUGIN:LoadDefaultConfig()
    self.Config.Messages = self.Config.Messages or {}
	self.Config.Messages.NoPermission = self.Config.Messages.NoPermission or "You do not have permission to use this command!"
    self.Config.Messages.NoTarget = self.Config.Messages.NoTarget or "Syntax: /info <player>"
	self.Config.Messages.PlayerNotFound = self.Config.Messages.PlayerNotFound or "Player not found!"
	self.Config.Messages.MultiplePlayers = self.Config.Messages.MultiplePlayers or "Multiple users found: "
	self.Config.Messages.ChatPrefix = self.Config.Messages.ChatPrefix or "P-INFO"
    self.Config.Messages.HisSteamId = self.Config.Messages.HisSteamId or "His/Her Steam ID is:"
	self.Config.Messages.YourSteamIdIs = self.Config.Messages.YourSteamIdIs or "Your Steam ID is:"
	self.Config.Messages.YourIpIs = self.Config.Messages.YourIpIs or "Your IP is:"
	self.Config.Messages.HisIPis = self.Config.Messages.HisIPis or "His/Her IP is:"
	self.Config.Messages.YourPingIs = self.Config.Messages.YourPingIs or "Your PING is:"
	self.Config.Messages.HisPingIs = self.Config.Messages.HisPingIs or "His/Her PING is:"
	self.Config.Messages.HisName = self.Config.Messages.HisName or "Player Name:"
	self.Config.Messages.YourPositionIs = self.Config.Messages.YourPositionIs or "Your Position is:"
	self.Config.Messages.HisPositionIs = self.Config.Messages.HisPositionIs or "His/Her Position is:"
	self.Config.Messages.PlayerIsMuted = self.Config.Messages.PlayerIsMuted or "This player is MUTED!"
	self.Config.Messages.PlayerIsNotMuted = self.Config.Messages.PlayerIsNotMuted or "This player is NOT MUTED!"
	self.Config.Messages.YouAreMuted = self.Config.Messages.YouAreMuted or "You are MUTED!"
	self.Config.Messages.YouAreNotMuted = self.Config.Messages.YouAreNotMuted or "You are NOT MUTED!"
	self.Config.Messages.PlayerIsAfk = self.Config.Messages.PlayerIsAfk or "This player is AFK !"
	self.Config.Messages.PlayerIsNotAfk = self.Config.Messages.PlayerIsNotAfk or "This player is NOT AFK! "
	self.Config.Messages.ResearchForUser = self.Config.Messages.ResearchForUser or "[Research For User]"
    self:SaveConfig()
end

local function Ping(connection) return Network.Net.sv:GetAveragePing(connection) end

-- SELF COMMAND /myinfo
function PLUGIN:cmdMyInfo(player, command, arg)
    local position = "X: " .. math.ceil(player.transform.position.x) .. ", Y: " .. math.ceil(player.transform.position.y) .. ", Z: " .. math.ceil(player.transform.position.z)
    rust.SendChatMessage(player, self.Config.Messages.ChatPrefix, "<color=aqua>[Self-Research]</color>")
	rust.SendChatMessage(player, self.Config.Messages.ChatPrefix, self.Config.Messages.YourSteamIdIs .." ".. rust.UserIDFromPlayer(player))
	rust.SendChatMessage(player, self.Config.Messages.ChatPrefix, self.Config.Messages.YourIpIs .." "..  player.net.connection.ipaddress)
	local ping = Ping(player.net.connection)
	rust.SendChatMessage(player, self.Config.Messages.ChatPrefix, self.Config.Messages.YourPingIs .." ".. ping)
	rust.SendChatMessage(player, self.Config.Messages.ChatPrefix, self.Config.Messages.YourPositionIs .." "..  position)
			local isMuted = ChatMute:Call("IsMuted", player) 
			if isMuted then
			rust.SendChatMessage(player, self.Config.Messages.ChatPrefix, self.Config.Messages.YouAreMuted)
			else
			rust.SendChatMessage(player, self.Config.Messages.ChatPrefix, self.Config.Messages.YouAreNotMuted)

		end
	end

-- ADMIN COMMAND /ainfo
function PLUGIN:cmdAinfo(player, cmd, args)
    local steamId = rust.UserIDFromPlayer(player)
	local position = "X: " .. math.ceil(player.transform.position.x) .. ", Y: " .. math.ceil(player.transform.position.y) .. ", Z: " .. math.ceil(player.transform.position.z)
    if not permission.UserHasPermission(steamId, "cancheckinfo") then
        rust.SendChatMessage(player, self.Config.Messages.NoPermission)
        return
    end
    if args.Length == 0 then
	rust.SendChatMessage(player, self.Config.Messages.ChatPrefix, self.Config.Messages.NoTarget)
	end
	if args.Length == 1 then
	
	local targetPlayer = tostring(args[0])
	local targetPlayer = self:findPlayer(targetPlayer)
	
	if #targetPlayer == 0 then
		rust.SendChatMessage(player, self.Config.Messages.ChatPrefix, self.Config.Messages.PlayerNotFound)
		elseif #targetPlayer > 1 then
			rust.SendChatMessage(player, self.Config.Messages.ChatPrefix, self.Config.Messages.MultiplePlayers)
			for _, matchingplayer in pairs(targetPlayer) do
					rust.SendChatMessage(player, "<color=green>-> </color>" ..  matchingplayer)
				end	
			elseif #targetPlayer == 1 then
			local targetPlayer = global.BasePlayer.Find(targetPlayer[1])
			local ping = Ping(targetPlayer.net.connection)
			rust.SendChatMessage(player, self.Config.Messages.ChatPrefix, "<color=cyan>[Research For User]</color>" )
			rust.SendChatMessage(player, self.Config.Messages.ChatPrefix, self.Config.Messages.HisName .." ".. targetPlayer.displayName )
			rust.SendChatMessage(player, self.Config.Messages.ChatPrefix, self.Config.Messages.HisSteamId .." ".. rust.UserIDFromPlayer(targetPlayer))
			player:SendConsoleCommand('echo <color=aqua>*********[PaiN Info Research Results!]*********</color>')
			player:SendConsoleCommand('echo Name:' .." ".. targetPlayer.displayName)
			player:SendConsoleCommand('echo IP:' .." ".. targetPlayer.net.connection.ipaddress)
			player:SendConsoleCommand('echo SteamID:' .." ".. rust.UserIDFromPlayer(targetPlayer))
			player:SendConsoleCommand('echo Ping:' .." ".. ping)
			player:SendConsoleCommand('echo Position:' .." ".. position)
			player:SendConsoleCommand('echo <color=aqua>***********************************************</color>')
			print('********[PaiN Info Research Results!]********')
			print('Name:' .." ".. targetPlayer.displayName)
			print('IP:' .." ".. targetPlayer.net.connection.ipaddress)
			print('SteamID:' .." ".. rust.UserIDFromPlayer(targetPlayer))
			print('Ping:' .." ".. ping)
			print('Position:' .." ".. position)
			print('*********************************************')
			rust.SendChatMessage(player, self.Config.Messages.ChatPrefix, self.Config.Messages.HisIPis .." ".. targetPlayer.net.connection.ipaddress)
			rust.SendChatMessage(player, self.Config.Messages.ChatPrefix, self.Config.Messages.HisPingIs .." ".. ping)
			rust.SendChatMessage(player, self.Config.Messages.ChatPrefix, self.Config.Messages.HisPositionIs .." ".. position)
			local isAfk = AFKSystem:Call("IsAFK", targetPlayer)
			if isAfk then  
			rust.SendChatMessage(player, self.Config.Messages.ChatPrefix, self.Config.Messages.PlayerIsAfk)
			else
			rust.SendChatMessage(player, self.Config.Messages.ChatPrefix, self.Config.Messages.PlayerIsNotAfk)
			end
		    local isMuted = ChatMute:Call("IsMuted", targetPlayer)
			if isMuted then
			rust.SendChatMessage(player, self.Config.Messages.ChatPrefix, self.Config.Messages.PlayerIsMuted)
			else
			rust.SendChatMessage(player, self.Config.Messages.ChatPrefix, self.Config.Messages.PlayerIsNotMuted)
			end
		end
	end
end

function PLUGIN:findPlayer(searchedPlayer)
	if not searchedPlayer then return end
	
	searchedPlayer = string.lower(searchedPlayer)
	
	local foundPlayers = {}
	local players = global.BasePlayer.activePlayerList:GetEnumerator()
	
	while players:MoveNext() do
		
		local currentPlayer = string.lower(players.Current.displayName)
		local currentDisplay = players.Current.displayName
		
		if string.find(currentPlayer, searchedPlayer, 1, true) then
			table.insert(foundPlayers, currentDisplay)
		end
	end
	return foundPlayers
end