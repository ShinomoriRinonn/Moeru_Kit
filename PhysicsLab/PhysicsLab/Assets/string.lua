local stringPractice = {}

function xyd.stringStartsWith(s1, s2)
    return string.find(s1, s2) == 1
end

function xyd.stringContains(s1, s2)
    return string.find(s1, s2) ~= nil
end

return stringPractice