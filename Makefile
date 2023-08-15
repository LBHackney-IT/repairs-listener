.PHONY: setup
setup:
	docker-compose build

.PHONY: build
build:
	docker-compose build repairs-listener

.PHONY: serve
serve:
	docker-compose build repairs-listener && docker-compose up repairs-listener

.PHONY: shell
shell:
	docker-compose run repairs-listener bash

.PHONY: test
test:
	docker-compose up dynamodb-database & docker-compose build repairs-listener-test && docker-compose up repairs-listener-test

.PHONY: lint
lint:
	-dotnet tool install -g dotnet-format
	dotnet tool update -g dotnet-format
	dotnet format

.PHONY: restart-db
restart-db:
	docker stop $$(docker ps -q --filter ancestor=dynamodb-database -a)
	-docker rm $$(docker ps -q --filter ancestor=dynamodb-database -a)
	docker rmi dynamodb-database
	docker-compose up -d dynamodb-database
