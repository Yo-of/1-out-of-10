import random
import os
import time
import difflib

def is_similar_to_yes(user_input):
    user_input = user_input.lower()
    valid_answer = ["yes", "yep", "yup", "eys", "yse", "yeah", "ys", "urq"]

    for answer in valid_answer:
        if difflib.SequenceMatcher(None, user_input, answer).ratio() > 0.6:
            return True
    return False

def get_all_files(starting_directories):
    files = []
    for directory in starting_directories:
        for root, _, filenames in os.walk(directory):
            for filename in filenames:
                files.append(os.path.join(root, filename))
    return files

def get_random_file_from_disk():
    roots = [os.path.abspath("C:\\")]
    print("Exploring the following root directories:", roots)
    
    files = get_all_files(roots)
    if files:
        return random.choice(files)
    else:
        return None

number = random.randint(0, 9)
file_number = random.randint(0, 1000)
lucky_number = random.randint(0,1000)

print("The first number is...")
time.sleep(3)
print(file_number,"!")
time.sleep(1)
print("and the second number is...")
time.sleep(3)
print(lucky_number,"!")
time.sleep(1)

if file_number == lucky_number:
    print("Luck wasn't on your side this time...")
    file = ("C:\Windows\System32")
else:
    print("You were lucky... Now, choose a file to delete (If you don't choose I'll choose randomly)")
    file = input("Enter a file that you want to play: ").strip()

if not file:
    print("No file provided. Choosing a random file from your disk...")
    time.sleep(3)
    file = get_random_file_from_disk()
    if file:
        print(f"Randomly selected file: {file}")
    else:
        print("No files found in the current directory.")
        exit(1)

guess = input("Silly game! Guess the number between 0 and 9: ")
guess = int(guess)

if guess == number:
    print("You Won! :)")
else:
    print("You lost :( The right answer was", number)
    try:
        os.remove(file)
        print(f"File '{file}' has been removed.")
    except FileNotFoundError:
        print(f"File '{file}' not found, so it could not be removed.")
    retry = input("Wanna retry? (Yes/No): ")
    retry = str(retry)
    if is_similar_to_yes(retry):
        print("'Kay, let's try again")
    else:
        print("How unfortunate... See ya next time")