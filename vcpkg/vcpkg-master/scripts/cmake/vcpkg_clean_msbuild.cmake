#[===[.md:
# vcpkg_clean_msbuild

Clean intermediate files generated by `vcpkg_install_msbuild()`.

## Usage
```cmake
vcpkg_clean_msbuild()
```

## Examples

* [xalan-c](https://github.com/Microsoft/vcpkg/blob/master/ports/xalan-c/portfile.cmake)
#]===]

function(vcpkg_clean_msbuild)
    file(REMOVE_RECURSE
        ${CURRENT_BUILDTREES_DIR}/${TARGET_TRIPLET}-dbg
        ${CURRENT_BUILDTREES_DIR}/${TARGET_TRIPLET}-rel
    )
endfunction()