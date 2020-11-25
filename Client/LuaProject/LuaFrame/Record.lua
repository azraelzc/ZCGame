local Record = class("Record")
Record.me = {
	__index = function ( vTable, vKey )
		return vTable[vKey]()
	end
}
function Record:initialize()
	Record.Clear(self)
end

function Record:Clear()
	self.Container = {}
	self.Count = 0
end

function Record:ContainsKey( vKey )
	local find = self.Container[vKey]
	return find ~= nil
end

function Record:Add( vKey, vValue)
	if not self:ContainsKey(vKey) then
		self.Count = self.Count + 1
	else
		error("Fatal error:all ready have vKey[" .. tostring(vKey) .. "] can not Add to record");
		return
	end

	self.Container[vKey] = vValue
end

function Record:Set( vKey, vValue)
	if not self:ContainsKey(vKey) then
		self.Count = self.Count + 1
	else
		--print("perform set action on vKey" .. tostring(vKey) .. " vValue:" .. tostring(vValue))
	end

	self.Container[vKey] = vValue
end

function Record:Remove( vKey )
	if self:ContainsKey(vKey) then
		self.Count = self.Count - 1
	else
		error("remove encouter error for key:" .. tostring(vKey))
	end

	self.Container[vKey] = nil
end

 function Record:Enumerator()
 	if not self then
 		error("self = nil")
 	end
   	return pairs(self.Container)
 end

 function Record:GetCount()
   	return self.Count
 end   

 function Record:GetValue(vKey)
   	return self.Container[vKey]
 end  

 function Record:GetValues()
   	local values = {}
   	for k,v in self:Enumerator() do
   		table.insert(values, v)
   	end
   	return values
 end  

 function Record:GetKeys( ... )
   	local keys = {}
   	for k,v in self:Enumerator() do
   		table.insert(keys, k)
   	end
   	return keys
 end  

function Record:Log()
	for k,v in self:Enumerator() do
		print(tostring(k) .. " : " .. tostring(v))
	end
end
    
--[[	
	
	local record =  Record()

	-- Add
	record:Add("key0", "value0")
	record:Add("key1", "value1")
	record:Add("key2", "value2")

	-- Set
	record:Set("key0", "value_0")

	-- Get value
	print(record:GetValue("key0"))
	
	-- Get record count
	print("record count(after init):" .. record:GetCount())

	-- Enumerate record all key and value
	for k,v in record:Enumerator() do
		print(k .. " -- " .. v)
	end
	
	-- Remove value
	record:Remove("key1")
--]]

return Record