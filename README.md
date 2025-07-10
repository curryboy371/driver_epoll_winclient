# 리눅스 드라이버를 곁들인 채팅서버

### 참고 링크

[유튜브](https://www.youtube.com/watch?v=SdQxKhBPPqQ)


## 1. 프로젝트 필요성 및 목적
1. 라즈베리파이 리눅스에서 I/O 효율성을 극대화한 대규모 네트워크 서버 구축
2. 직접 작성한 리눅스 디바이스 드라이버와 유저 클라이언트와 연결
3. 멀티 플랫폼 환경에서의 패킷 통신과 유저 인증, 세션 처리 등을 경험


## 2. 시스템 구성
<img width="1159" height="708" alt="image" src="https://github.com/user-attachments/assets/a84b9dc0-f4aa-4129-b722-8edba82fe410" />


## 3. 통신 플로우
<img width="1159" height="594" alt="image" src="https://github.com/user-attachments/assets/fc5a9ddb-cf01-4a76-9817-4fe5cb5282f3" />



## 4. thread 구조 및 패킷 구성

1. Main Tread ( Event 처리 )
<img width="579" height="797" alt="image" src="https://github.com/user-attachments/assets/29555eb1-255a-42e9-b152-47f10f8e5318" />

2. Sub Thread ( Send, Recv 분산 처리 )
<img width="890" height="731" alt="image" src="https://github.com/user-attachments/assets/5999240f-177f-49c8-a6c2-c933f7c5090b" />

3. 패킷 구조 ( Protobuf 사용 )
<img width="722" height="356" alt="image" src="https://github.com/user-attachments/assets/ac5f02af-c022-400c-92b4-0d1b14efc980" />


## 5. 상세 수행 내용

### I2C 리눅스 디바이스 드라이버
LCD1602 
 I2C core + char dev 방식, /dev/lcd1602 write로 문자열 출력 가능, udev 연동해 자동 노드 생성

BMP180 
 I2C core + sysfs 드라이버,  /sys/.../temperature 파일에서 보정된 온도/기압값 read. 
 probe 시 EEPROM 보정 테이블 자동 로딩.

### Linux Epoll 서버 + MongoDB + Protobuf
Epoll 기반 I/O로 설계하여 수천 개 동시 접속 처리 가능.
Main thread는 epoll 이벤트 감지 전담, Sub thread는 Task Queue를 통해 수신·송신을 mutex + condition variable로 thread-safe하게 분산 처리.
recv 버퍼를 사용해 TCP stream 특성상 데이터가 잘리거나 여러 패킷이 붙어 도착하는 상황에서도 헤더 기반으로 완전한 패킷 단위로 복원.
MongoDB로 사용자 로그인 및 세션 관리, uthash로 유저 데이터 Hash Map으로 관리.
Protobuf로 패킷 직렬화(역직렬화), C ↔ C# 플랫폼 간 동일한 패킷으로 통신할 수 있도록 개발.

### WinForm 클라이언트 + Protobuf
.NET Form으로 로그인, 가입, 채팅 UI 구현.
TCP 소켓으로 서버 연결, Protobuf 패킷 수신 후 invoke로 UI 스레드 안전하게 업데이트.

### 채팅방 주요 기능
유저 로그인 및 중복 로그인 방지, 신규 가입 처리
닉네임 변경 시 모든 유저에게 변경 알림 전송
채팅 메시지 송수신 및 유저 입장·퇴장 시 알림
커맨드 명령어를 클라이언트가 선별해 별도 패킷으로 전송해 서버 부하를 줄임
명령어를 통해 서버에서 device driver(LCD, BMP180)에 직접 접근 후,
클라이언트에게 결과 알림 및 드라이버 기능(온도 표시, 문자열 출력 등) 수행


## 6. 문제점과 개선방향

### 1) WinForm에서 TCP 소켓 수신 → UI Thread 충돌
WinForm은 UI 컨트롤이 생성된 스레드(주로 UI thread)에서만 접근 가능.
TCP 수신은 별도 스레드에서 동작하기 때문에 ListBox 등에 바로 Add 시 예외 발생.
→ Invoke를 사용해 UI thread에 안전하게 델리게이트를 전달해 처리.

### 2) Deadlock: 유저 Broadcast + 유저 제거 과정

문제 상황
유저가 나갈 때 user_manager의 mutex lock을 잡고, 해시맵에서 유저를 삭제.
동시에 브로드캐스트를 위해 user list를 얻어 다시 같은 mutex를 lock, send 처리.
같은 mutex를 재귀적으로 걸면서 데드락 발생.

해결
유저 제거시 lock을 짧게 유지, 이후 User List를 불러오고 braodcast 하는 방식으로 해결함.
그러나 User가 많아지면 현재 처리중인 thread가 혼자서 많은 일을 처리하게 된다는 사실을 발견.

유저 목록만 복사 후, send task만 enqueue하고 작업중인 thread는 빠져나오도록 처리
실제 송신은 thread pool이 분산 처리 → IO block을 최소화

추가로 Read, Write Lock을 구분하여, Read 상황만 있다면 여러 Thread가 함께 Lock을 걸고 사용할 수 있도록 추후 개선하는 방법도 고려



