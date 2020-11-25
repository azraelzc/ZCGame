local setmetatableindex_
setmetatableindex_ = function(t, index)
    if type(t) == "userdata" then
        -- local peer = tolua.getpeer(t)
        -- if not peer then
        --     peer = {}
        --     tolua.setpeer(t, peer)
        -- end
        -- setmetatableindex_(peer, index)
    else
        local mt = getmetatable(t)
        if not mt then mt = {} end
        if not mt.__index then
            mt.__index = index
            setmetatable(t, mt)
        elseif mt.__index ~= index then
            setmetatableindex_(mt, index)
        end
    end
end

local setmetatableindex = setmetatableindex_

local Class = {}
function Class.build(classname, ...)
    if not classname or type(classname) == "table" then
        assert(false, "error class definition!")
    end
    local cls = {name = classname}

    local supers = {...}
    for _, super in ipairs(supers) do
        local superType = type(super)
        assert(superType == "nil" or superType == "table" or superType == "function",
            string.format("class() - create class \"%s\" with invalid super class type \"%s\"",
                classname, superType))

        if superType == "function" then
            assert(cls.__create == nil,
                string.format("class() - create class \"%s\" with more than one creating function",
                    classname));
            -- if super is function, set it to __create
            cls.__create = super
        elseif superType == "table" then
            if super[".isclass"] then
                -- super is native class
                assert(cls.__create == nil,
                    string.format("class() - create class \"%s\" with more than one creating function or native class",
                        classname));
                cls.__create = function() return super:create() end
            else
                -- super is pure lua class
                cls.__supers = cls.__supers or {}
                cls.__supers[#cls.__supers + 1] = super
                if not cls.super then
                    -- set first super pure lua class as class.super
                    cls.super = super
                end
            end
        else
            error(string.format("class() - create class \"%s\" with invalid super type",
                classname), 0)
        end
    end

    cls.__index = cls
    if not cls.__supers or #cls.__supers == 1 then
        setmetatable(cls, {__index = cls.super})
    else
        setmetatable(cls, {__index = function(_, key)
            local supers = cls.__supers
            for i = 1, #supers do
                local super = supers[i]
                if super[key] then return super[key] end
            end
        end})
    end

    if not cls.initialize then
        -- add default construinitialize
        cls.initialize = function() end
    end
    cls.new = function(...)
        local instance
        if cls.__create then
            instance = cls.__create(...)
        else
            instance = {}
        end
        setmetatableindex(instance, cls)
        instance.class = cls
        instance:initialize(...)
        return instance
    end


    cls.create = function(_, ...)
        return cls.new(...)
    end

    cls.is = function(obj, cls)
        if not obj or not cls then 
            return false 
        end

        local __index = rawget(cls, "__index")
        local tarCName = rawget(__index, "name")

        return iskindof(obj, tarCName)
    end

    cls.log = function(ctx, msg, ...)
        local param = {...}
        for i, v in ipairs(param) do param[i] = tostring(v) end
        local msg = string.format(tostring(msg), unpack(param))
        print("[" .. ctx.name .. "] " .. msg)
    end

    local mt = getmetatable(cls)
    mt.__call = function( _,... )
    	-- body
    	return cls.new(...)
    end
    setmetatable(cls, mt)
    -- setmetatable(cls, { __call = function(_, ...) return cls.new(...) end })
    return cls
end

return Class.build
