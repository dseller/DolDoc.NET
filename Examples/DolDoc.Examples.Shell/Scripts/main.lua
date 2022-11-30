function PrintHelp()
	print('$TR,"Input help"$\n')
	print('$ID,2$Prefix your command with "$FG,GREEN$#$FG$" to evaluate a Lua expression.\n\n')
	print('  $FG,LTGRAY$ > # for i=1,10 do print(math.pi*i .. "\\n") end$FG$\n\n')
	print("Built-in commands are executed first. After that, the input is sent to the OS to be executed.\n")
	print('$ID,-2$$TR,"Commands"$\n')
	print('$ID,2$ * $FG,GREEN$dir$FG$ Directory listing\n')
	print('$ID,-2$\n')
end

print('\n\n$FG,CYAN$$TX+CX+B,"TempleShell v0.1-alpha"$$FG$\n')
PrintHelp()
print("Lua version " .. _VERSION .. "\n")

STATE = {
	pwd = WORKING_DIRECTORY
}

commands = {
	ls = function()
		entries = directory_listing(STATE.pwd)
		local buffer = "$FG,LTBLUE$Directory of " .. STATE.pwd .. "\n"
		buffer = buffer .. "DATE       TIME  SIZE NAME\n"
		
		for i=0, entries.Length - 1 do
			entry = entries[i]
			buffer = buffer .. 
				entry.LastModified:ToString("yyyy/MM/dd HH:mm") .. " " .. 
				string.format("%04X", math.min(entry.Size, 0xFFFF)) .. " "
				-- entry.Name .. "\n"

			if entry.IsDirectory then
				buffer = buffer .. string.format('$MA,"%s",LE="STATE.pwd = \'%s\'; commands.ls(); RenderPrompt()"$\n', entry.Name, entry.FullPath)
			else
				buffer = buffer .. string.format('$LK,"%s",A="%s"$\n', entry.Name, entry.FullPath)
			end
		end

		buffer = buffer .. "$FG$\n"
		print(buffer)
	end
}

function OnMacro(entry)
	-- print(entry:GetArgument("LE"))

	local code = entry:GetArgument("LE")
	eval(code)
end

function RenderPrompt()
	print(string.format("\n\n%s> $FG,CYAN$$PT$", STATE.pwd))
end

function OnPrompt(str)
	print("$FG$\n\n")
	local cmd = commands[str]
	if cmd == nil then
		if str:sub(1, 1) == "#" then
			print(eval(str:sub(2)))
		else
	        local bla = io.popen(str, "r")
	        bla:setvbuf("line")
			print(bla:read("*a"))
		end
	else
		cmd()
	end
	
	RenderPrompt()
end

-- Render initial prompt.
RenderPrompt()
