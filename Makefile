
build:
	docker rmi mono-app
	docker build -t mono-app .

run:
	docker run --rm mono-app mono ConsoleApplication1.exe
