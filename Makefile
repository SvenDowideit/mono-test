
build:
	docker rmi mono-app
	docker build -t mono-app .

run:
	docker rm -vf nancyapp || true
	docker run -d -p 8000:8000 --name nancyapp \
		mono-app mono ConsoleApplication1.exe
