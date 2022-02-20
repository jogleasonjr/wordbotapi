# WordBot.Api

A quick hack Wordle helper for a Telegram Bot. It simply filters large lists of words based on user input.

### Usage

The API has a single GET endpoint that accepts parameters for `guess`, which is the current row if your Wordle board, and `omit` (optional) which is a list of characters that cannot be in the final word list.

For the `guess` parameter characters, uppercase indicates exact positioning (green squares in Wordle), and lowercase can anywhere (yellow squares in Wordle). Spaces are accepted, including trailing spaces. There's no assumption of a 5 character limit, so pad the `guess` parameter with spaces if needed.

For example, if your last row in Wordle looks like this:

## [S] [ ] [`R`] [`L`] [ ]

And the keyboard has greyed out the characters `xptbhcwyieu`, you would make the following GET request:

`http://localhost:5000/search?guess=S%20rl%20&omit=xptbhcwyieu`

Note the capital `S`, which indicates it's fixed in position zero, while `r` and `l` can appear anywhere in the results. This would have the response:

```
English Word List 1 [370,103 words]
 1. salar
 2. snarl
 3. solar
 4. soral

Google 10,000 Common English Words [9,894 words]
 1. solar
```

You could also skip the `omit` parameter and just call `http://localhost:5000/search?guess=S%20rl%2`, which would simply return a larger list.