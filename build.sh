cd ./src/AppMetrics
./build.sh

if [ $? -ne 0 ]; then
    echo "An error occured."
    exit 1
fi

cd ../..

cd ./src/AspNetCore.Storage
./build.sh

if [ $? -ne 0 ]; then
    echo "An error occured."
    exit 1
fi

cd ../..