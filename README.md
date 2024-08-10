## Elasticsearch, MongoDB, ve Graylog Konteynerlerinin Çalıştırılması

Bu adımlar, Elasticsearch, MongoDB ve Graylog'u Docker konteynerleri olarak indirip çalıştırmak için kullanılır.

### Elasticsearch Konteyneri
Elasticsearch'i Docker konteyneri olarak çalıştırmak için aşağıdaki komutu terminalde çalıştırın:

```bash
docker run -d --name elasticsearch \
  -p 9200:9200 -p 9300:9300 \
  -e "discovery.type=single-node" \
  -e "xpack.security.enabled=false" \
  -v esdata:/usr/share/elasticsearch/data \
  docker.elastic.co/elasticsearch/elasticsearch:8.9.0

```

### MongoDB Konteyneri

```bash
docker run -d --name mongo \
  -p 27017:27017 \
  -v mongodata:/data/db \
  mongo:6.0
```



### Graylog Konteyneri

```bash
docker run -d --name graylog \
  --link mongo --link elasticsearch \
  -p 9000:9000 \
  -p 12201:12201/udp \
  -e "GRAYLOG_PASSWORD_SECRET=somepasswordpepper" \
  -e "GRAYLOG_ROOT_PASSWORD_SHA2=SHA256ofYourPassword" \
  -e "GRAYLOG_HTTP_EXTERNAL_URI=http://127.0.0.1:9000/" \
  -e "GRAYLOG_ELASTICSEARCH_HOSTS=http://elasticsearch:9200" \
  -v graylogdata:/usr/share/graylog/data \
  graylog/graylog:5.2
```

### Servislere Erişim
Elasticsearch: http://localhost:9200
MongoDB: mongodb://localhost:27017
Graylog: http://localhost:9000

Graylog arayüzüne tarayıcınızdan http://localhost:9000 adresinden erişebilirsiniz.
