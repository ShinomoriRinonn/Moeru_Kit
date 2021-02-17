# Automatically generated by boost-vcpkg-helpers/generate-ports.ps1

vcpkg_from_github(
    OUT_SOURCE_PATH SOURCE_PATH
    REPO boostorg/move
    REF boost-1.74.0
    SHA512 f1ef4320b0a4f9618ff93a2fdb98aa52e5d4d9fcaafc031e16517d5f3ee2aea99736a20ac2b9289e390cc6ea26364f1d50fb51bf34f9574bbb360591677b35fe
    HEAD_REF master
)

include(${CURRENT_INSTALLED_DIR}/share/boost-vcpkg-helpers/boost-modular-headers.cmake)
boost_modular_headers(SOURCE_PATH ${SOURCE_PATH})