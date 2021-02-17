# Automatically generated by boost-vcpkg-helpers/generate-ports.ps1

vcpkg_from_github(
    OUT_SOURCE_PATH SOURCE_PATH
    REPO boostorg/fiber
    REF boost-1.74.0
    SHA512 ede0f770fcdd7f56b70b6a86de8d246b8abbcc9a0608b3d19c582a1240811d46320a5eb9c9416c83fc358dfd5b17855ab17f4e965af5c522b9b08c5a6df8675c
    HEAD_REF master
)

file(READ "${SOURCE_PATH}/build/Jamfile.v2" _contents)
string(REPLACE "import ../../config/checks/config" "import config/checks/config" _contents "${_contents}")
file(WRITE "${SOURCE_PATH}/build/Jamfile.v2" "${_contents}")
file(COPY "${CURRENT_INSTALLED_DIR}/share/boost-config/checks" DESTINATION "${SOURCE_PATH}/build/config")

include(${CURRENT_INSTALLED_DIR}/share/boost-build/boost-modular-build.cmake)
boost_modular_build(SOURCE_PATH ${SOURCE_PATH})
include(${CURRENT_INSTALLED_DIR}/share/boost-vcpkg-helpers/boost-modular-headers.cmake)
boost_modular_headers(SOURCE_PATH ${SOURCE_PATH})